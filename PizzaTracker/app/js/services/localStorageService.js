﻿(function () { "use strict"; var a = angular.module("LocalStorageModule", []); a.provider("localStorageService", function () { this.prefix = "ls", this.storageType = "localStorage", this.cookie = { expiry: 30, path: "/" }, this.notify = { setItem: !0, removeItem: !1 }, this.setPrefix = function (a) { this.prefix = a }, this.setStorageType = function (a) { this.storageType = a }, this.setStorageCookie = function (a, b) { this.cookie = { expiry: a, path: b } }, this.setStorageCookieDomain = function (a) { this.cookie.domain = a }, this.setNotify = function (a, b) { this.notify = { setItem: a, removeItem: b } }, this.$get = ["$rootScope", "$window", "$document", function (a, b, c) { var d, e = this.prefix, f = this.cookie, g = this.notify, h = this.storageType; c || (c = document), "." !== e.substr(-1) && (e = e ? e + "." : ""); var i = function (a) { return e + a }, j = function () { try { var c = h in b && null !== b[h], e = i("__" + Math.round(1e7 * Math.random())); return c && (d = b[h], d.setItem(e, ""), d.removeItem(e)), c } catch (f) { return h = "cookie", a.$broadcast("LocalStorageModule.notification.error", f.message), !1 } }(), k = function (b, c) { if (!j) return a.$broadcast("LocalStorageModule.notification.warning", "LOCAL_STORAGE_NOT_SUPPORTED"), g.setItem && a.$broadcast("LocalStorageModule.notification.setitem", { key: b, newvalue: c, storageType: "cookie" }), q(b, c); "undefined" == typeof c && (c = null); try { (angular.isObject(c) || angular.isArray(c)) && (c = angular.toJson(c)), d && d.setItem(i(b), c), g.setItem && a.$broadcast("LocalStorageModule.notification.setitem", { key: b, newvalue: c, storageType: this.storageType }) } catch (e) { return a.$broadcast("LocalStorageModule.notification.error", e.message), q(b, c) } return !0 }, l = function (b) { if (!j) return a.$broadcast("LocalStorageModule.notification.warning", "LOCAL_STORAGE_NOT_SUPPORTED"), r(b); var c = d ? d.getItem(i(b)) : null; return c && "null" !== c ? "{" === c.charAt(0) || "[" === c.charAt(0) ? angular.fromJson(c) : c : null }, m = function (b) { if (!j) return a.$broadcast("LocalStorageModule.notification.warning", "LOCAL_STORAGE_NOT_SUPPORTED"), g.removeItem && a.$broadcast("LocalStorageModule.notification.removeitem", { key: b, storageType: "cookie" }), s(b); try { d.removeItem(i(b)), g.removeItem && a.$broadcast("LocalStorageModule.notification.removeitem", { key: b, storageType: this.storageType }) } catch (c) { return a.$broadcast("LocalStorageModule.notification.error", c.message), s(b) } return !0 }, n = function () { if (!j) return a.$broadcast("LocalStorageModule.notification.warning", "LOCAL_STORAGE_NOT_SUPPORTED"), !1; var b = e.length, c = []; for (var f in d) if (f.substr(0, b) === e) try { c.push(f.substr(b)) } catch (g) { return a.$broadcast("LocalStorageModule.notification.error", g.Description), [] } return c }, o = function (b) { b = b || ""; var c = e.slice(0, -1), f = new RegExp(c + "." + b); if (!j) return a.$broadcast("LocalStorageModule.notification.warning", "LOCAL_STORAGE_NOT_SUPPORTED"), t(); var g = e.length; for (var h in d) if (f.test(h)) try { m(h.substr(g)) } catch (i) { return a.$broadcast("LocalStorageModule.notification.error", i.message), t() } return !0 }, p = function () { try { return navigator.cookieEnabled || "cookie" in c && (c.cookie.length > 0 || (c.cookie = "test").indexOf.call(c.cookie, "test") > -1) } catch (b) { return a.$broadcast("LocalStorageModule.notification.error", b.message), !1 } }, q = function (b, d) { if ("undefined" == typeof d) return !1; if (!p()) return a.$broadcast("LocalStorageModule.notification.error", "COOKIES_NOT_SUPPORTED"), !1; try { var e = "", g = new Date, h = ""; if (null === d ? (g.setTime(g.getTime() + -864e5), e = "; expires=" + g.toGMTString(), d = "") : 0 !== f.expiry && (g.setTime(g.getTime() + 24 * f.expiry * 60 * 60 * 1e3), e = "; expires=" + g.toGMTString()), b) { var j = "; path=" + f.path; f.domain && (h = "; domain=" + f.domain), c.cookie = i(b) + "=" + encodeURIComponent(d) + e + j + h } } catch (k) { return a.$broadcast("LocalStorageModule.notification.error", k.message), !1 } return !0 }, r = function (b) { if (!p()) return a.$broadcast("LocalStorageModule.notification.error", "COOKIES_NOT_SUPPORTED"), !1; for (var d = c.cookie && c.cookie.split(";") || [], f = 0; f < d.length; f++) { for (var g = d[f]; " " === g.charAt(0) ;) g = g.substring(1, g.length); if (0 === g.indexOf(i(b) + "=")) return decodeURIComponent(g.substring(e.length + b.length + 1, g.length)) } return null }, s = function (a) { q(a, null) }, t = function () { for (var a = null, b = e.length, d = c.cookie.split(";"), f = 0; f < d.length; f++) { for (a = d[f]; " " === a.charAt(0) ;) a = a.substring(1, a.length); var g = a.substring(b, a.indexOf("=")); s(g) } }, u = function () { return h }, v = function (a, b, c) { var d = l(b); null === d && angular.isDefined(c) ? d = c : angular.isObject(d) && angular.isObject(c) && (d = angular.extend(c, d)), a[b] = d, a.$watchCollection(b, function (a) { k(b, a) }) }; return { isSupported: j, getStorageType: u, set: k, add: k, get: l, keys: n, remove: m, clearAll: o, bind: v, deriveKey: i, cookie: { set: q, add: q, get: r, remove: s, clearAll: t } } }] }) }).call(this);