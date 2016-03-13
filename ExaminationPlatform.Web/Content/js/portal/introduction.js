var baseOp = {
    init_Objects: function () {
        $scope.batchId = getQueryStringByName("id");
    },
    init_ExamInfo: function () {
        if ($scope.paperId) {
            $http.get("/api/ExaminationAPI/GetExamInfo?batchId=" + $scope.batchId).success(function (result) {
                if (result.IsSuccess) {
                    $scope.examInfo = result.Data;
                }
                else {
                    switch (result.ErrorCode) {
                        case "100":
                            alert("用户信息失效，请重新登陆");
                            location.href = "/Auth";
                            break;
                        case "200":
                            alert("考试尚未开始");
                            break;
                        case "201":
                            alert("考试已结束");
                            break;
                        case "300":
                            alert("已无试卷，请与管理人员联系");
                            break;
                        default:
                            alert("未知错误");
                    }

                }
            });
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
    }
};

//初始化
baseOp.init_Objects();
baseOp.init_Events();
baseOp.init_ExamInfo();