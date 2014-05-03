app.controller("mainController", function($scope, $http, $modal, $q) {
    $scope.threads = [];
    $scope.threadIds = [];
    $scope.blogs = [];
    $scope.currentBlog = null;
    $scope.currentBlogId = 0;
    $scope.currentTurn = 'true';
    $scope.showNewBlogForm = false;
    $scope.loading = true;
    $scope.orderBy = "";
    $scope.orderReverse = false;
    $scope.filterSearch = null;
    $scope.showAlert = true;
    $scope.latestNews = null;
    $scope.init = function() {
        $scope.loading = true;
        $scope.threads = [];
        $scope.blogs = [];
        GetBlogs()
            .then(GetThreadIds)
            .then(GetThreads)
            .then(function() {
                $scope.loading = false;
            });
        GetLatestNews();
    };

    var GetBlogs = function() {
        var deferred = $q.defer();
        $http.get("/Home/GetBlogs").success(function(data) {
            $scope.blogs = data;
            deferred.resolve();
        }).error(function(error) {

        });
        return deferred.promise;
    };
    var GetThreadIds = function() {
        var deferred = $q.defer();
        $http.get("/Home/GetThreadIds").success(function (data) {
            $scope.threadIds = data;
            deferred.resolve();
        }).error(function (error) {

        });
        return deferred.promise;;
    };
    var GetThreads = function() {
        var deferred = $q.defer();
        var promises = [];
        angular.forEach($scope.threadIds, function(value, key) {
            promises.push(GetThread(value));
        });
        $q.all(promises).then(function() {
            deferred.resolve();
        });
        return deferred.promise;
    };
    var GetThread = function(id) {
        var deferred = $q.defer();
        $http.get("/Home/GetThread?threadId=" + id).success(function(data2) {
            $scope.threads.push(data2);
            deferred.resolve();
        });
        return deferred.promise;
    };
    var GetLatestNews = function () {
        var deferred = $q.defer();
        $http.get("/Home/GetLatestNews").success(function (data) {
            $scope.latestNews = data;
            deferred.resolve();
        }).error(function (error) {

        });
        return deferred.promise;;
    };
    $scope.setCurrentBlog = function(blogShortname, blogId) {
        blogShortname = blogShortname || null;
        blogId = blogId || null;
        $scope.currentBlog = blogShortname;
        $scope.currentBlogId = blogId;
    };
    $scope.setCurrentTurn = function(currentTurn) {
        currentTurn = currentTurn || null;
        $scope.currentTurn = currentTurn;
    };

    $scope.setOrderBy = function(thread) {
        switch ($scope.orderBy) {
        case "userTitle":
            return thread.UserTitle;
            break;
        case "lastPostDate":
            return thread.LastPostDate;
            break;
        case "lastPosterShortname":
            return thread.LastPosterShortname;
            break;
        default:
            return thread.UserThreadId;
            break;
        }
    };

    $scope.openNewThread = function() {

        var modalInstance = $modal.open({
            templateUrl: '/Static/NewThreadModal.html?v=' + new Date().getTime(),
            controller: 'ModalInstanceCtrl',
            resolve: {
                'blogs': function() { return $scope.blogs; }
            }
        });
        modalInstance.result.then(function(response) {

        }, function() {

        });
    };
});

app.controller('ModalInstanceCtrl', function($scope, $modalInstance, blogs) {

    $scope.blogs = blogs;
    $scope.cancel = function() {
        $modalInstance.close();
    };
});

app.filter('isCorrectBlog', function() {
    return function(input, blogToCompare) {
        if (blogToCompare === null || blogToCompare === '') {
            return input;
        }
        var out = [];
        for (var i = 0; i < input.length; i++) {
            if (input[i].BlogShortname == blogToCompare) {
                out.push(input[i]);
            }
        }
        return out;
    };
});
app.filter('isCorrectTurn', function() {
    return function(input, turnToCompare) {
        if (turnToCompare == 'false') {
            turnToCompare = false;
        } else if (turnToCompare == 'true') {
            turnToCompare = true;
        }
        if (turnToCompare === null || turnToCompare === '') {
            return input;
        }
        var out = [];
        for (var i = 0; i < input.length; i++) {
            if (input[i].IsMyTurn == turnToCompare) {
                out.push(input[i]);
            }
        }
        return out;
    };
});