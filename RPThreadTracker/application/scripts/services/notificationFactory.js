'use strict';
(function() {
	angular.module('rpthreadtracker')
        .factory('TrackerNotification',
		[
			'Notification', TrackerNotification
		]);

	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function TrackerNotification(Notification) {
		function NotificationInstance() {
			this.options = {};
		}

		NotificationInstance.prototype.withTitle = function(title) {
			this.options.title = title;
			return this;
		};
		NotificationInstance.prototype.withMessage = function(message) {
			this.options.message = message;
			return this;
		};
		NotificationInstance.prototype.appendMessage = function(message) {
			if (this.options.message) {
				this.options.message += '<br />';
			}
			this.options.message += message;
			return this;
		};
		NotificationInstance.prototype.withTemplateUrl = function(templateUrl) {
			this.options.templateUrl = templateUrl;
			return this;
		};
		NotificationInstance.prototype.withDelay = function(delay) {
			this.options.delay = delay;
			return this;
		};
		NotificationInstance.prototype.withType = function(type) {
			this.options.type = type;
			return this;
		};
		NotificationInstance.prototype.withPositionY = function(positionY) {
			this.options.positionY = positionY;
			return this;
		};
		NotificationInstance.prototype.withPositionX = function(positionX) {
			this.options.positionX = positionX;
			return this;
		};
		NotificationInstance.prototype.withReplaceMessage = function(replaceMessage) {
			this.options.replaceMessage = replaceMessage;
			return this;
		};
		NotificationInstance.prototype.withCloseOnClick = function(closeOnClick) {
			this.options.closeOnClick = closeOnClick;
			return this;
		};
		NotificationInstance.prototype.show = function() {
			if (!this.options.type) {
				this.options.type = 'primary';
			}
			return Notification(this.options, this.options.type);
		};
		return NotificationInstance;
	}
}());
