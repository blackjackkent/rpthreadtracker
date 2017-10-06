// When the extension is installed or upgraded ...
chrome.runtime.onInstalled.addListener(function () {
	// Replace all rules ...
	chrome.declarativeContent.onPageChanged.removeRules(undefined, function () {
		// With a new rule ...
		chrome.declarativeContent.onPageChanged.addRules([{
			// That fires when a page's URL contains a 'g' ...
			conditions: [
				new chrome.declarativeContent.PageStateMatcher({
					pageUrl: { urlMatches: '(http|https):\/\/(.*\.tumblr.com|.*\.co\.vu)\/post\/*' },
				})
			],
			// And shows the extension's page action.
			actions: [new chrome.declarativeContent.ShowPageAction()]
		}]);
	});
});

chrome.pageAction.onClicked.addListener(openTracker);

function openTracker(tab) {
	var a = document.createElement('a');
	a.href = tab.url;
	var postId = a.pathname.split('/')[2];
	var blogShortname = a.hostname.split('.')[0];
	chrome.windows.create({
		url: 'http://www.rpthreadtracker.com/add-thread?tumblrBlogShortname=' + blogShortname + '&tumblrPostId=' + postId + '&addFromExtension=true',
		left: 50,
		top: 50,
		width: 1200,
		height: 800,
		focused: true,
		type: 'popup'
	});
}
