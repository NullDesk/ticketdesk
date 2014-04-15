/** 
 * @module Authentication module. 
 *         All the interactions with users should start from this module
 * @requires system
 * @requires router
 * @requires routeconfig
 * @requires utils
 */

define(["durandal/system","durandal/app","plugins/router","services/routeconfig", "services/utils"]
	,function (system, app, router, routeconfig, utils, history) {

	var self = this;

	// Build external login url
	function externalLoginsUrl(returnUrl, generateState) {
		return "/api/account/externalLogins?returnUrl=" + (encodeURIComponent(returnUrl)) +
			"&generateState=" + (generateState ? "true" : "false");
	};

	// Build manage info url
	function manageInfoUrl(returnUrl, generateState) {
		return "/api/account/manageinfo?returnUrl=" + (encodeURIComponent(returnUrl)) +
			"&generateState=" + (generateState ? "true" : "false");
	};

	// Get the Authorization header for include in requests
	function getSecurityHeaders() {
		var accessToken = sessionStorage["accessToken"] || localStorage["accessToken"];

		if (accessToken) {
			return { "Authorization": "Bearer " + accessToken };
		}

		return {};
	};

	// Get url fragments
	function getFragment() {
		if (window.location.hash.indexOf("#") === 0) {
			return parseQueryString(window.location.hash.substr(1));
		} else {
			return {};
		}
	};
		
	// Set query string parameters to an object
	function parseQueryString(queryString) {
		var data = {},
			pairs, pair, separatorIndex, escapedKey, escapedValue, key, value;

		if (queryString === null) {
			return data;
		}

		pairs = queryString.split("&");

		for (var i = 0; i < pairs.length; i++) {
			pair = pairs[i];
			separatorIndex = pair.indexOf("=");

			if (separatorIndex === -1) {
				escapedKey = pair;
				escapedValue = null;
			} else {
				escapedKey = pair.substr(0, separatorIndex);
				escapedValue = pair.substr(separatorIndex + 1);
			}

			key = decodeURIComponent(escapedKey);
			value = decodeURIComponent(escapedValue);

			data[key] = value;
		}

		return data;
	};
	
	// Clear stored access tokens from local and session storage
	function clearAccessToken() {
		localStorage.removeItem("accessToken");
		sessionStorage.removeItem("accessToken");
	};

	// Set access tokens in local or session storage
	function setAccessToken(accessToken, persistent) {
		if (persistent) {
			localStorage["accessToken"] = accessToken;
		} else {
			sessionStorage["accessToken"] = accessToken;
		}
	};
		
	// Verify the returned state match with the stored one
	function verifyStateMatch(fragment) {
		var state;

		if (typeof (fragment.access_token) !== "undefined") {
			state = sessionStorage["state"];
			sessionStorage.removeItem("state");

			if (state === null || fragment.state !== state) {
				fragment.error = "invalid_state";
			}
		}
	};	
	
	// Cleanup location fragment
	function cleanUpLocation() {
		window.location.hash = "";

		if (history && typeof (history.pushState) !== "undefined") {
			history.pushState("", document.title, location.pathname);
		}
	};
	
	//Archive session storage to local
	function archiveSessionStorageToLocalStorage() {
		var backup = {};

		for (var i = 0; i < sessionStorage.length; i++) {
			backup[sessionStorage.key(i)] = sessionStorage[sessionStorage.key(i)];
		}

		localStorage["sessionStorageBackup"] = JSON.stringify(backup);
		sessionStorage.clear();
	};
		
	// Restore session storage from local
	function restoreSessionStorageFromLocalStorage() {
		var backupText = localStorage["sessionStorageBackup"],
			backup;

		if (backupText) {
			backup = JSON.parse(backupText);

			for (var key in backup) {
				sessionStorage[key] = backup[key];
			}

			localStorage.removeItem("sessionStorageBackup");
		}
	};	
	
	// Class representing user information
	function UserInfoViewModel(name, roles) {
		this.name = ko.observable(name);
		this.roles = ko.observableArray(roles);
	};

	return {

		/** property {object} userInfo - The user object */
		userInfo: ko.observable(),

		/**
		 * Set the authentication info. Look in storage for stored info
		 * param {string} userName
		 * param {Array} roles
		 * param {string} accessToken
		 * param {bool} persistent
		 */
		setAuthInfo: function (userName, roles, accessToken, persistent) {
			var self = this;

			if (accessToken) {
				setAccessToken(accessToken, persistent)
			}

			if (typeof (roles) == "string") {
				roles = roles.split(",");
			}
			self.userInfo(new UserInfoViewModel(userName, roles));
		},
		
		/**
		 * Remove authentication info
		 */
		clearAuthInfo: function () {
			var self = this;

			clearAccessToken();
			self.userInfo(undefined);
		},

		/**
		 * Check if the authenticathed user belongs to the role
		 * param {Array} roles
		 */
		isUserInRole : function (roles) {
			var self = this,
				isuserinrole = false;

			$.each(roles, function(key, value) {
				if (self.userInfo().roles.indexOf(value) !== -1) {
					isuserinrole = true;
				}
			});

			return isuserinrole;
		},

		/**
		 * Get the security headers for sending authenticated ajax requests to the server
		 */
		getSecurityHeaders: getSecurityHeaders,

		/**
		 * Helper method for storing authentication info in the Local Storage
		 */
		archiveSessionStorageToLocalStorage : archiveSessionStorageToLocalStorage,

		returnUrl : routeconfig.siteUrl,

		/**
		 * Add external Login
		 * param {object} data - External login info
		 */
		addExternalLogin : function (data) {
			return $.ajax(routeconfig.addExternalLoginUrl, {
				type: "POST",
				data: data,
				headers: getSecurityHeaders()
			});
		},

		/**
		 * Change the password
		 * param {object} data - Change password info
		 */
		changePassword : function (data) {
			return $.ajax(routeconfig.changePasswordUrl, {
				type: "POST",
				data: data,
				headers: getSecurityHeaders()
			});
		},

		/**
		 * Get the external logins list
		 * param {string} returnUrl
		 * param {bool} generateState - If true the server will generate a Guid for matching in the client
		 *                              when the authentication provider returns the authentication result
		 */
		getExternalLogins : function (returnUrl, generateState) {
			return $.ajax(externalLoginsUrl(returnUrl, generateState), {
				cache: false,
				headers: getSecurityHeaders()
			});
		},

		/**
		 * Get account info for managing the authenticated user data
		 * param {object} returnUrl
		 * param {bool} generateState - If true the server will generate a Guid for matching in the client
		 *                              when the authentication provider returns the authentication result
		 */
		getManageInfo : function (returnUrl, generateState) {
			return $.ajax(manageInfoUrl(returnUrl, generateState), {
				cache: false,
				headers: getSecurityHeaders()
			});
		},

		/**
		 * Get authenticated user info
		 * param {object} accessToken
		 */
		getUserInfo : function (accessToken) {
			var self = this,
				headers;

			if (typeof (accessToken) !== "undefined") {
				headers = {
					"Authorization": "Bearer " + accessToken
				};
			} else {
				headers = getSecurityHeaders();
			}

			return $.ajax(routeconfig.userInfoUrl, {
				cache: false,
				headers: headers
			});
		},

		/**
		 * Login the user
		 * param {object} data - Login info
		 */
		login : function (data) {
			return $.ajax(routeconfig.loginUrl, {
				type: "POST",
				data: data
			});
		},

		/**
		 * Logout the user
		 */
		logout : function () {
			return $.ajax(routeconfig.logoutUrl, {
				type: "POST",
				headers: getSecurityHeaders()
			});
		},

		/**
		 * Register new user
		 * param {object} data - Registration info
		 */
		register : function (data) {
			return $.ajax(routeconfig.registerUrl, {
				type: "POST",
				data: data
			});
		},

		/**
		 * Register external user
		 * param {string} accessToken
		 * param {object} data - Registration info
		 */
		registerExternal : function (accessToken, data) {
			return $.ajax(routeconfig.registerExternalUrl, {
				type: "POST",
				data: data,
				headers: {
					"Authorization": "Bearer " + accessToken
				}
			});
		},

		/**
		 * Remove login from the user
		 * param {object} data - Remove login info
		 */
		removeLogin : function (data) {
			return $.ajax(routeconfig.removeLoginUrl, {
				type: "POST",
				data: data,
				headers: getSecurityHeaders()
			});
		},

		/**
		 * Set authenticated user password
		 * param {object} data - Set password info
		 */
		setPassword : function (data) {
			return $.ajax(routeconfig.setPasswordUrl, {
				type: "POST",
				data: data,
				headers: getSecurityHeaders()
			});
		},
		
		/**
		 * Get a list of users
		 */
		getUsers: function () {
			return $.ajax(routeconfig.getUsersUrl, {
				type: "GET",
				headers: getSecurityHeaders()
			});
		},

		/**
		 * Always call this method when initializating the application for getting authenticated user info (from storage)
		 * or redirect when returning from a provider or associating another login
		 */
		initializeAuth : function() {
			var self = this;

			return system.defer(function (dfd) {
				var fragment = getFragment(),
					externalAccessToken, externalError, loginUrl;

				restoreSessionStorageFromLocalStorage();
				verifyStateMatch(fragment);

				if (sessionStorage["associatingExternalLogin"]) {
					sessionStorage.removeItem("associatingExternalLogin");

					if (typeof (fragment.error) !== "undefined") {
						externalAccessToken = null;
						externalError = fragment.error;
						cleanUpLocation();
					} else if (typeof (fragment.access_token) !== "undefined") {
						externalAccessToken = fragment.access_token;
						externalError = null;
						cleanUpLocation();
					} else {
						externalAccessToken = null;
						externalError = null;
						cleanUpLocation();
					}

					self.getUserInfo()
						.done(function (data) {
							if (data.userName) {
								self.setAuthInfo(data.userName, data.roles);
								sessionStorage["redirectTo"] = "account/manage?externalAccessToken=" + externalAccessToken + "&externalError=" + externalError;
							}
							dfd.resolve(true);
						})
						.fail(function () {
							dfd.resolve(true);
						});
				} else if (typeof (fragment.error) !== "undefined") {
					cleanUpLocation();
					sessionStorage["redirectTo"] = "account/externalloginfailure";
					dfd.resolve(true);					
				} else if (typeof (fragment.access_token) !== "undefined") {
					cleanUpLocation();
					self.getUserInfo(fragment.access_token)
						.done(function (data) {
							if (typeof (data.userName) !== "undefined" && typeof (data.hasRegistered) !== "undefined"
								&& typeof (data.loginProvider) !== "undefined") {
								if (data.hasRegistered) {
									// Change persistent to true for storing the authentication token in local storage when
									// login with external services
									self.setAuthInfo(data.userName, data.roles, fragment.access_token, false);
									dfd.resolve(true);
								}
								else if (typeof (sessionStorage["loginUrl"]) !== "undefined") {
									loginUrl = sessionStorage["loginUrl"];
									sessionStorage.removeItem("loginUrl");
									sessionStorage["redirectTo"] = "account/externalloginconfirmation?userName=" + data.userName +
														"&loginProvider=" + data.loginProvider +
														"&access_token=" + fragment.access_token +
														"&loginUrl=" + encodeURIComponent(loginUrl) +
														"&state=" + fragment.state;
									dfd.resolve(true);
								}
								else {
									dfd.resolve(true);
								}
							} else {
								dfd.resolve(true);
							}
						})
						.fail(function () {
							dfd.resolve(true);
						});
				// Checking for the access token in order to get the user info
				// It is necessary to avoid command prompt issues in Windows Phone. Check issue #12
				} else if (sessionStorage["accessToken"] || localStorage["accessToken"]) {
					self.getUserInfo()
						.done(function (data) {
							if (data.userName) {
								self.setAuthInfo(data.userName, data.roles);
							}
							dfd.resolve(true);
						})
						.fail(function () {
							dfd.resolve(true);
						});
				} else {
					dfd.resolve(true);
				}				
			}).promise();
		}
	};
});
