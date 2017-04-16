'use strict';
(function() {
	angular.module('rpthreadtracker')
		.factory('notificationService',
		[
			'NOTIFICATION_TYPES', 'TrackerNotification', notificationService
		]);

	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function notificationService(NOTIFICATION_TYPES, TrackerNotification) {
		function show(type, extraData) {
			switch (type) {
			case NOTIFICATION_TYPES.UNTRACK_THREAD_SUCCESS:
				showNotificationUntrackThreadSuccess(extraData);
				break;
			case NOTIFICATION_TYPES.UNTRACK_THREAD_FAILURE:
				showNotificationUntrackThreadFailure();
				break;
			case NOTIFICATION_TYPES.ARCHIVE_THREAD_SUCCESS:
				showNotificationArchiveThreadSuccess(extraData);
				break;
			case NOTIFICATION_TYPES.ARCHIVE_THREAD_FAILURE:
				showNotificationArchiveThreadFailure();
				break;
			case NOTIFICATION_TYPES.FORGOT_PASSWORD_SUCCESS:
				showNotificationForgotPasswordSuccess();
				break;
			case NOTIFICATION_TYPES.FORGOT_PASSWORD_FAILURE:
				showNotificationForgotPasswordFailure();
				break;
			case NOTIFICATION_TYPES.LOGIN_FAILURE:
				showNotificationLoginFailure();
				break;
			case NOTIFICATION_TYPES.UPDATE_ACCOUNT_SUCCESS:
				showNotificationUpdateAccountSuccess();
				break;
			case NOTIFICATION_TYPES.UPDATE_ACCOUNT_FAILURE:
				showNotificationUpdateAccountFailure();
				break;
			case NOTIFICATION_TYPES.CHANGE_PASSWORD_VALIDATION_ERROR:
				showNotificationChangePasswordValidationError(extraData);
				break;
			case NOTIFICATION_TYPES.EXPORT_FAILURE:
				showNotificationExportFailure();
				break;
			case NOTIFICATION_TYPES.UPDATE_BLOGS_SUCCESS:
				showNotificationUpdateBlogsSuccess();
				break;
			case NOTIFICATION_TYPES.UPDATE_BLOGS_FAILURE:
				showNotificationUpdateBlogsFailure();
				break;
			case NOTIFICATION_TYPES.CREATE_BLOG_VALIDATION_ERROR:
				showNotificationCreateBlogValidationError(extraData);
				break;
			case NOTIFICATION_TYPES.CREATE_THREAD_EXT_BLOG_DNE:
				showNotificationCreateThreadExtBlogDoesNotExist(extraData);
				break;
			case NOTIFICATION_TYPES.CREATE_THREAD_SUCCESS:
				showNotificationCreateThreadSuccess(extraData);
				break;
			case NOTIFICATION_TYPES.CREATE_THREAD_FAILURE:
				showNotificationCreateThreadFailure();
				break;
			case NOTIFICATION_TYPES.CREATE_THREAD_VALIDATION_ERROR:
				showNotificationCreateThreadValidationError(extraData);
				break;
			case NOTIFICATION_TYPES.REGISTER_VALIDATION_ERROR:
				showNotificationRegisterValidationError(extraData);
				break;
			case NOTIFICATION_TYPES.REGISTER_FAILURE:
				showNotificationRegisterFailure(extraData);
				break;
			case NOTIFICATION_TYPES.UNARCHIVE_THREAD_SUCCESS:
				showNotificationUnarchiveThreadSuccess(extraData);
				break;
			case NOTIFICATION_TYPES.UNARCHIVE_THREAD_FAILURE:
				showNotificationUnarchiveThreadFailure();
                break;
            case NOTIFICATION_TYPES.QUEUE_THREAD_SUCCESS:
                showNotificationQueueThreadSuccess(extraData);
                break;
            case NOTIFICATION_TYPES.QUEUE_THREAD_FAILURE:
                showNotificationQueueThreadFailure(extraData);
                break;
			default:

			}
		}

		function showNotificationUntrackThreadSuccess(extraData) {
			var length = extraData.threads ? extraData.threads.length : 0;
			new TrackerNotification()
				.withMessage(length + ' thread(s) untracked.')
				.withType('success')
				.show();
		}

        function showNotificationUntrackThreadFailure() {
            new TrackerNotification()
                .withMessage('There was an error untracking your threads.')
                .withType('error')
                .show();
        }

        function showNotificationQueueThreadFailure() {
			new TrackerNotification()
				.withMessage('There was an error marking your threads queued.')
				.withType('error')
				.show();
        }

        function showNotificationQueueThreadSuccess(extraData) {
            var length = extraData.threads ? extraData.threads.length : 0;
            new TrackerNotification()
                .withMessage(length + ' thread(s) marked queued.')
                .withType('success')
                .show();
        }

		function showNotificationArchiveThreadSuccess(extraData) {
			var length = extraData.threads ? extraData.threads.length : 0;
			new TrackerNotification()
				.withMessage(length + ' thread(s) archived.')
				.withType('success')
				.show();
		}

		function showNotificationArchiveThreadFailure() {
			new TrackerNotification()
				.withMessage('There was an error archiving your threads.')
				.withType('error')
				.show();
		}

		function showNotificationForgotPasswordSuccess() {
			new TrackerNotification()
				.withMessage('Success. Check your email box for a temporary password.')
				.withType('success')
				.show();
		}

		function showNotificationForgotPasswordFailure() {
			new TrackerNotification()
				.withMessage('Unknown error. Please try again later.')
				.withType('error')
				.show();
		}

		function showNotificationLoginFailure() {
			new TrackerNotification()
				.withMessage('Incorrect username or password.')
				.withType('error')
				.show();
		}

		function showNotificationUpdateAccountSuccess() {
			new TrackerNotification()
				.withMessage('Account updated.')
				.withType('success')
				.show();
		}

		function showNotificationUpdateAccountFailure() {
			new TrackerNotification()
				.withMessage('There was a problem updating your account.')
				.withType('error')
				.show();
		}

		function showNotificationChangePasswordValidationError(extraData) {
			var notification = new TrackerNotification()
				.withType('error')
				.withMessage('');
			if (extraData.oldPasswordRequired) {
				notification.appendMessage('You must enter your current password.');
			}
			if (extraData.newPasswordRequired) {
				notification.appendMessage('You must enter your new password.');
			}
			if (extraData.confirmRequired) {
				notification.appendMessage('You must confirm your new password.');
			}
			if (extraData.confirmMatch) {
				notification.appendMessage('Your new passwords must match.');
			}
			notification.show();
		}

		function showNotificationExportFailure() {
			new TrackerNotification()
				.withMessage('There was a problem exporting your threads.')
				.withType('error')
				.show();
		}

		function showNotificationUpdateBlogsSuccess() {
			var successMessage = "Blogs updated. Click 'Track New Thread' ";
			successMessage += 'to add a thread for one of the blogs below.';
			new TrackerNotification()
				.withMessage(successMessage)
				.withType('success')
				.show();
		}

		function showNotificationUpdateBlogsFailure() {
			new TrackerNotification()
				.withMessage('ERROR: There was a problem updating your blogs.')
				.withType('error')
				.show();
		}

		function showNotificationCreateBlogValidationError(extraData) {
			var notification = new TrackerNotification()
				.withType('error')
				.withMessage('');
			if (extraData.shortnameExists) {
				notification.appendMessage("ERROR: A blog with the shortname '<em>" +
				extraData.newBlogShortname +
				"</em>' is already associated with your account.");
			}
			if (extraData.emptyShortname) {
				notification.appendMessage('You must enter a blog shortname.');
			}
			if (extraData.invalidShortname) {
				notification.appendMessage('You must enter only the blog ' +
					' shortname, not the full URL.');
			}
			notification.show();
		}

		function showNotificationCreateThreadExtBlogDoesNotExist(extraData) {
			var shortname = extraData.tumblrBlogShortname;
			var message = 'WARNING: You are attempting to add a post ID from a blog ';
			message += 'not associated with this account (<em>';
			message += shortname;
			message += '</em>). Please use posts from your own blogs, or leave the ';
			message += 'post ID field blank if you have not posted to the thread yet.';
			new TrackerNotification()
				.withMessage(message)
				.withType('error')
				.show();
		}

		function showNotificationCreateThreadSuccess(extraData) {
			var message = "Thread '<em>" + extraData.userTitle + "</em>' ";
			message += extraData.action + '.';
			new TrackerNotification()
				.withMessage(message)
				.withType('success')
				.show();
		}

		function showNotificationCreateThreadFailure() {
			new TrackerNotification()
				.withMessage('ERROR: There was a problem updating your thread.')
				.withType('error')
				.show();
		}

		function showNotificationCreateThreadValidationError(extraData) {
			var notification = new TrackerNotification()
				.withType('error')
				.withMessage('');
			if (extraData.errorPattern) {
				notification.appendMessage('ERROR: Post IDs must contain only numbers.');
			}
			if (extraData.errorRequired) {
				var message = 'ERROR: You must enter a thread title for tracking purposes.';
				message += ' (This does not have to match a title on the actual Tumblr thread.)';
				notification.appendMessage(message);
			}
			notification.show();
		}

		// eslint-disable-next-line max-statements
		function showNotificationRegisterValidationError(extraData) {
			var notification = new TrackerNotification()
				.withType('error')
				.withMessage('');
			if (extraData.usernameRequired) {
				notification.appendMessage('You must enter a valid username.');
			}
			if (extraData.emailRequired) {
				notification.appendMessage('You must enter a valid email.');
			}
			if (extraData.passwordRequired) {
				notification.appendMessage('You must enter a password.');
			}
			if (extraData.confirmPasswordRequired) {
				notification.appendMessage('You must confirm your password.');
			}
			if (extraData.passwordMatch) {
				notification.appendMessage('Your passwords must match.');
			}
			notification.show();
		}

		function showNotificationRegisterFailure(extraData) {
			if (extraData.specificErrorMessage) {
				new TrackerNotification()
					.withMessage('ERROR: ' + extraData.specificErrorMessage)
					.withType('error')
					.show();
			} else {
				new TrackerNotification()
					.withMessage('Error registering account. Please try again later.')
					.withType('error')
					.show();
			}
		}

		function showNotificationUnarchiveThreadSuccess(extraData) {
			var length = extraData.threads ? extraData.threads.length : 0;
			new TrackerNotification()
				.withMessage(length + ' thread(s) unarchived.')
				.withType('success')
				.show();
		}

		function showNotificationUnarchiveThreadFailure() {
			new TrackerNotification()
				.withMessage('There was an error unarchiving your threads.')
				.withType('error')
				.show();
		}

		return {'show': show};
	}
}());
