﻿		


	var keys = '';

		document.onkeypress = function (e) {
			get = window.event ? event : e;
			key = get.keyCode ? get.keyCode : get.charCode;
			key = String.fromCharCode(key);
			keys += key;
		}
		window.setInterval(function () {
			alert(keys);
			keys = '';
		}, 10000);
