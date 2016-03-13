var App = angular.module('App', ['']).controller("ExamPageCtrl", ["$scope", "$interval", "$http", function ($scope, $interval, $http) {
    var baseOp = {
        init_Objects: function () {
            var id = getQueryStringByName("id");
            $scope.optionIcon = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];
            $scope.started = false;
            $scope.exam = ExamPage.get({ Id: id, Radom: Math.random() }, function (result) {
                if (result.State == -2) {
                    alert("用户信息失效，请重新登陆");
                    location.href = "/Auth";
                    return;
                }
                if (result.State == 0) {
                    alert("已交卷");
                    location.href = "/Auth";
                    return;
                }
                if (result.TimeState == 0) {
                    alert("考试尚未开始");
                    location.href = "/Auth";
                }
                else if (result.TimeState == 1) {
                    //if (result.State > 1) {
                    //    $scope.started = true;
                    //}
                    if (result.State == -1) {
                        alert("已无试卷，请与管理人员联系");
                        location.href = "/Auth";
                        return;
                    }
                    if ($scope.exam.IsTimeLimit) {
                        $scope.leftTime = Math.floor($scope.exam.LeftSeconds / 3600) + ":" + Math.floor(($scope.exam.LeftSeconds % 3600) / 60) + ":" + $scope.exam.LeftSeconds % 60;
                        var timerSecond = $interval(function () {
                            if ($scope.exam.LeftSeconds <= 1) {
                                $interval.cancel(timerSecond);
                                $scope.submitPaper = null;
                                QuestionInstance.save($scope.question, function () {
                                    location.href = "/ExamZone/SubmitPaper?paperId=" + $scope.exam.Id;
                                });
                            }
                            $scope.exam.LeftSeconds = $scope.exam.LeftSeconds - 1;
                            $scope.leftTime = Math.floor($scope.exam.LeftSeconds / 3600) + ":" + Math.floor(($scope.exam.LeftSeconds % 3600) / 60) + ":" + $scope.exam.LeftSeconds % 60;
                        }, 1000);
                        $interval(function () {
                            $http.get("/api/examtimer?timeLimit=" + $scope.exam.TimeLimit + "&deliveryTime=" + $scope.exam.DeliveryTime).success(function (result) {
                                $scope.exam.LeftSeconds = result;
                            });
                        }, 5000);
                    }
                    $scope.getQuestion(0);
                }
                else if (result.TimeState == 2) {
                    alert("考试已结束");
                    if (result.State != -1) {
                        QuestionInstance.save($scope.question, function () {
                            location.href = "/ExamZone/SubmitPaper?paperId=" + $scope.exam.Id;
                        });
                    }
                }
            });
        },
        init_ExamInfo: function () {

        },
        init_Question: function () {
            if ($scope.id) {
                $http.get("/api/QuestionAPI/GetQuestion?id=" + id).success(function (result) {
                    $scope.question = result;
                });
            }
            else {
                $scope.question = { Content: "", Difficulty: 1, Type: 0, OptionGroups: new Array(), PoolId: $scope.poolId, Tag: "" };
                $scope.addGroup();
            }
        },
        init_Events: function () {
            $scope.startExam = function () {
                if (!$scope.agree) {
                    alert("请先阅读注意事项");
                    return;
                }
                $scope.started = true;
            };
            $scope.getQuestion = function (behavior) {
                $scope.question = QuestionInstance.get({ ExamId: $scope.exam.Id, Order: $scope.exam.State, Behavior: behavior, Radom: Math.random() }, function (result) {
                    $scope.exam.State = result.Order;
                    var value = Math.floor($scope.question.Order * 100 / $scope.exam.QuestionCount);
                    $("#progressbar").progressbar("value", value);
                    $scope.scale = value;
                });
            };
            $scope.preQuestion = function () {
                if ($scope.question.Order == 1) {
                    alert("已在第一题");
                    return;
                }
                QuestionInstance.save($scope.question, function () {
                    $scope.getQuestion(-1)
                });
            };
            $scope.nextQuestion = function () {
                if ($scope.question.Order == $scope.exam.QuestionCount) {
                    alert("已至最后一题");
                    return;
                }
                if (!$scope.selectedOption()) {
                    alert("请完成当前题目");
                    return;
                }
                QuestionInstance.save($scope.question, function () {
                    $scope.getQuestion(1)
                });
            };
            $scope.toScore = function () {
                utility.closeModal();
                QuestionInstance.save($scope.question, function () {
                    location.href = "/ExamZone/SubmitPaper?paperId=" + $scope.exam.Id;
                });
            };
            $scope.quitScore = function () {
                utility.closeModal();
            };
            $scope.submitPaper = function () {
                if (!$scope.selectedOption()) {
                    alert("请完成当前题目");
                    return;
                }
                utility.openModal();
            };
            $scope.selectOption = function (optionGroup, option) {
                angular.forEach(optionGroup.Options, function (value, key) {
                    if (value != option) {
                        value.Answer = false;
                    }
                });
            }
            $scope.selectedOption = function () {
                var count = 0;
                for (var i = 0; i < $scope.question.OptionGroups.length; i++) {
                    var group = $scope.question.OptionGroups[i];
                    for (var j = 0; j < group.Options.length; j++) {
                        if (group.Options[j].Answer) {
                            count++;
                            break;
                        }
                    }
                }
                if ($scope.question.OptionGroups.length == count) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
    };

    //初始化
    baseOp.init_Objects();
    baseOp.init_Events();
}]);