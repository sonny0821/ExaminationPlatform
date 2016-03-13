var $ = (function () {
	
	"use strict"
	
    var _xhr = new XMLHttpRequest();
	
	var _ajax = function (settings) {
		
		var defaults = {
			url: null,
			type: "GET",
			data: {},
			dataType: "text",
			async: true,
			success: function () { },
			failure: function () { },
			single: true
		};
		
		var options = _extend(defaults, settings);
		
		if (_xhr.readyState > 1 && _xhr.readyState < 4 && options.single) return;
		
		if (options.type == "GET") {
			options.url = _makeGetRequestUrl(options.url, options.data);
		}
		
		_xhr.open(options.type, options.url, options.async);
		
		_xhr.onreadystatechange = function () {
			if (_xhr.readyState > 3) {
				if (_xhr.status == 200) {
					options.success(_xhr.responseText);
				} else {
				    options.failure(_xhr.responseText);
				}
			}
		}
		
		if (options.type == "POST") {
		    var formData = _makeQueryString(options.data);
		    _xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
		    _xhr.send(formData);
		} else {
			_xhr.send();
		}
	}
	
	var _extend = function(defaults, settings){
		for (var key in settings){
			if (settings[key]){
				defaults[key] = settings[key];
			}
		}
		return defaults;
	}
	
	var _makeGetRequestUrl = function (url, data) {
	    if (data == null) return url;
	    var params = /\?/.test(url) ? "&" : "?";
	    params += _makeQueryString(data)
	    return url + params;
	}
	
	var _makeQueryString = function (data) {
	    var params = new Array();
	    for (var key in data) {
			params.push(key + "=" + data[key]);
	    }
	    return params.join("&");
	}
	
	var _get = function (url, data, success) {
	    if (typeof (data) == "function") {
	        success = data;
	        data = null;
	    }
		_ajax({
			url: url,
			type: "GET",
			data: data,
			success: success
		});
	}
	
	var _post = function (url, data, success) {
	    if (typeof (data) == "function") {
	        success = data;
	        data = null;
	    }
		_ajax({
			url: url,
			type: "POST",
			data: data,
			success: success
		});
	}
	
	var _getJSON = function (url, data, success) {
	    if (typeof (data) == "function") {
	        success = data;
	        data = null;
	    }
		_ajax({
			url: url,
			type: "GET",
			data: data,
			success: function (responseText) {
				try {
					if (responseText.length > 1) {
						success(JSON.parse(responseText));
					}
				} catch (e) {
					alert(e.message);
				}
			}
		});
	}

	return {
		ajax: _ajax,
		get: _get,
		getJSON: _getJSON,
		post: _post,
		extend: _extend
	}
})();