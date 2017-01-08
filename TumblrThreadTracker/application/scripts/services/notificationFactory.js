'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.services
    .factory('TrackerNotification', [
    'Notification', function(Notification) {
        var TrackerNotification = function() {
            this.options = {};
        }

        TrackerNotification.prototype.withTitle = function(title) {
            this.options.title = title;
            return this;
        }

        TrackerNotification.prototype.withMessage = function(message) {
            this.options.message = message;
            return this;
        }

        TrackerNotification.prototype.appendMessage = function (message) {
            if (this.options.message) {
                this.options.message += "<br />";
            }
            this.options.message += message;
            return this;
        }

        TrackerNotification.prototype.withTemplateUrl = function(templateUrl) {
            this.options.templateUrl = templateUrl;
            return this;
        }

        TrackerNotification.prototype.withDelay = function(delay) {
            this.options.delay = delay;
            return this;
        }

        TrackerNotification.prototype.withType = function(type) {
            this.options.type = type;
            return this;
        }

        TrackerNotification.prototype.withPositionY = function(positionY) {
            this.options.positionY = positionY;
            return this;
        }

        TrackerNotification.prototype.withPositionX = function(positionX) {
            this.options.positionX = positionX;
            return this;
        }

        TrackerNotification.prototype.withReplaceMessage = function(replaceMessage) {
            this.options.replaceMessage = replaceMessage;
            return this;
        }

        TrackerNotification.prototype.withCloseOnClick = function(closeOnClick) {
            this.options.closeOnClick = closeOnClick;
            return this;
        }

        TrackerNotification.prototype.show = function () {
            if (!this.options.type) {
                this.options.type = "primary";
            }
            return Notification(this.options, this.options.type);
        }

        return TrackerNotification;
    }
]);