browser.tabs.onUpdated.addListener((tabId, changeInfo, tab) => {
  if (tab.url.match(/(http|https):\/\/.*\.tumblr.com\/post\/\d*\/.*/)) {
    browser.pageAction.show(tab.id);
  }
});
browser.pageAction.onClicked.addListener(openTracker);

function openTracker(tab) {
    var a = document.createElement('a');
    a.href = tab.url;
    var postId = a.pathname.split('/')[2];
    var blogShortname = a.hostname.split('.')[0];
    browser.windows.create({
        url: 'http://www.rpthreadtracker.com/add-thread?tumblrBlogShortname=' + blogShortname + '&tumblrPostId=' + postId + '&addFromExtension=true',
        left: 50,
        top: 50,
        width: 1200,
        height: 800,
        type: 'popup'
    });
}