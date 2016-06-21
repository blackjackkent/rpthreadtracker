//eFINJbEClrYtHDSiEIberyJA7BYcS43u7GopVsZlGGFlrb1r49 - key
//FlgsLqZnRfYycAQ0t8dB2GI6IwkaeDHYaL53tqDhrTOU2XOvtb - secret

$(document).ready(function() {
	var TrackerExtensionRunner = function() {
		function run() {
			console.log('test');
		}
		return {
			run: run
		};
	}
	new TrackerExtensionRunner().run();
});