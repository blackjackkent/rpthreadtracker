app.controller("mainController", function ($scope, $http, $modal) {
    $scope.threads = [];
    $scope.blogs = [];
    $scope.currentBlog = null;
    $scope.currentTurn = 'true';
    $scope.showNewBlogForm = false;
    $scope.loading = true;
    $scope.orderBy = "";
    $scope.orderReverse = false;
    $scope.filterSearch = null;
    $scope.init = function () {
        $http.get("/Home/GetThreads").success(function (data) {
            $scope.threads = data.Threads;
            console.log(data.Threads);
            $scope.blogs = data.UserBlogs;
            $scope.loading = false;
        }).error(function (error) {

        });
    };
    $scope.setCurrentBlog = function(blogShortname) {
        blogShortname = blogShortname || null;
        $scope.currentBlog = blogShortname;
    };
    $scope.setCurrentTurn = function(currentTurn) {
        currentTurn = currentTurn || null;
        $scope.currentTurn = currentTurn;
    };

    $scope.setOrderBy = function (thread) {
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

    $scope.openNewThread = function () {

        var modalInstance = $modal.open({
            templateUrl: '/Static/NewThreadModal.html',
            controller: 'ModalInstanceCtrl',
            resolve: {
                'blogs': function () { return $scope.blogs; }
            }
        });
        modalInstance.result.then(function (response) {

        }, function () {

        });
    };
});

app.controller('ModalInstanceCtrl', function ($scope, $modalInstance, blogs) {

    $scope.blogs = blogs;
    $scope.cancel = function () {
        $modalInstance.close();
    };
});

app.filter('isCorrectBlog', function() {
    return function (input, blogToCompare) {
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
    return function (input, turnToCompare) {
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