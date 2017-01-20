(function() {
    "use strict";
    angular.module("rpthreadtracker")
        .factory("TrackerNotification",
        [
            "Notification", TrackerNotification
        ]);

    function TrackerNotification(Notification) {
        var notificationInstance = function() {
            this.options = {};
        };
        notificationInstance.prototype.withTitle = function(title) {
            this.options.title = title;
            return this;
        };
        notificationInstance.prototype.withMessage = function(message) {
            this.options.message = message;
            return this;
        };
        notificationInstance.prototype.appendMessage = function(message) {
            if (this.options.message) {
                this.options.message += "<br />";
            }
            this.options.message += message;
            return this;
        };
        notificationInstance.prototype.withTemplateUrl = function(templateUrl) {
            this.options.templateUrl = templateUrl;
            return this;
        };
        notificationInstance.prototype.withDelay = function(delay) {
            this.options.delay = delay;
            return this;
        };
        notificationInstance.prototype.withType = function(type) {
            this.options.type = type;
            return this;
        };
        notificationInstance.prototype.withPositionY = function(positionY) {
            this.options.positionY = positionY;
            return this;
        };
        notificationInstance.prototype.withPositionX = function(positionX) {
            this.options.positionX = positionX;
            return this;
        };
        notificationInstance.prototype.withReplaceMessage = function(replaceMessage) {
            this.options.replaceMessage = replaceMessage;
            return this;
        };
        notificationInstance.prototype.withCloseOnClick = function(closeOnClick) {
            this.options.closeOnClick = closeOnClick;
            return this;
        };
        notificationInstance.prototype.show = function() {
            if (!this.options.type) {
                this.options.type = "primary";
            }
            return Notification(this.options, this.options.type);
        };
        return notificationInstance;
    }
})();