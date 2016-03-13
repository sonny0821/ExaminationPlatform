(function () {
    var baseOp = {
        //数据源及操作方法
        init_Objects: function () {
            this.papers = {
                search: function (filter, callback) {
                    var questions = this;
                    $.ajax({
                        url: "/api/Question",
                        type: "GET",
                        dataType: "json",
                        data: filter,
                        success: function (result) {
                            if (result.IsSuccess) {
                                if (callback instanceof Function) {
                                    callback(result.Data);
                                }
                            }
                        }
                    });
                },
                add: function (poolId) {
                    location.href = "/Admin/Question/Info?poolId=" + poolId;
                },
                edit: function (id) {
                    location.href = "/Admin/Question/Info?id=" + id;
                },
                remove: function (id, type, callback) {
                    $.ajax({
                        url: "/api/Question?id=" + id + "&type=" + type,
                        type: "DELETE",
                        dataType: "json",
                        success: function (result) {
                            if (result.IsSuccess) {
                                if (callback instanceof Function) {
                                    callback();
                                }
                            }
                        }
                    });
                }
            }
        },
        //试题表
        init_TemplateTable: function () {
            var templateTableClass = React.createClass({
                getInitialState: function () {
                    return { rowCount: 20, currentIndex: 1 };
                },
                searchQuestions: function () {
                    var that = this,
                        rowCount = that.state.rowCount,
                        currentIndex = that.state.currentIndex,
                        filter = {
                            name: that.refs.searchQuestionName.value,
                            poolIds: baseOp.pools.selectedPoolIds.join(),
                            pageIndex: currentIndex,
                            rowCount: rowCount
                        };
                    baseOp.questions.search(filter, function (data) {
                        var temp = data.QuestionCount / rowCount;
                        var intTemp = Math.floor(temp);
                        that.setState({ source: data, pageCount: intTemp + (temp == intTemp ? 0 : 1) });
                    });
                },
                pageQuestions: function (pageIndex) {
                    var that = this,
                        rowCount = that.state.rowCount,
                        filter = {
                            name: that.refs.searchQuestionName.value,
                            poolIds: baseOp.pools.selectedPoolIds.join(),
                            pageIndex: pageIndex,
                            rowCount: rowCount
                        };
                    baseOp.questions.search(filter, function (data) {
                        var temp = data.QuestionCount / rowCount;
                        var intTemp = Math.floor(temp);
                        that.setState({ source: data, pageCount: intTemp + (temp == intTemp ? 0 : 1), currentIndex: pageIndex });
                    });
                },
                render: function () {
                    var that = this,
                        rowCount = that.state.rowCount,
                        currentIndex = that.state.currentIndex,
                        pageCount = that.state.pageCount,
                        data = that.state.source,
                        questionClass = React.createClass({
                            editQuestion: function () {
                                baseOp.questions.edit(this.props.source.Id);
                            },
                            deleteQuestion: function () {
                                baseOp.questions.remove(this.props.source.Id, this.props.source.Type, function () {
                                    that.searchQuestions();
                                });
                            },
                            render: function () {
                                var question = this.props.source;
                                return React.DOM.tr(null,
                                    React.DOM.td(null,question),
                                    React.DOM.td(null, question.TypeValue),
                                    React.DOM.td(null, question.Tag),
                                    React.DOM.td(null, question.QuestionPoolName),
                                    React.DOM.td(null,
                                        ReactExtend.DOM.button({ type: "primary", content: "编辑", onClick: this.editQuestion }),
                                        ReactExtend.DOM.button({ type: "danger", content: "删除", className: "margin-left-10", onClick: this.deleteQuestion })
                                    )
                                );
                            }
                        });
                    if (data) {
                        var questionsDOM = data.Questions.map(function (question, index) {
                            return React.createElement(questionClass, {
                                source: question, key: index, index: index, deleteQuestion: function (index) {
                                    baseOp.questions.remove(questions[index].Id, function () {
                                        questions.splice(index, 1);
                                    });
                                }
                            });
                        });
                    }
                    return (
                        React.DOM.div({ className: "row" },
                            React.DOM.div({ className: "col-md-offset-8 col-md-4" },
                                React.DOM.div({ className: "input-group" },
                                    React.DOM.input({ type: "text", className: "form-control", ref: "searchQuestionName" }),
                                    React.DOM.span({ className: "input-group-btn" },
                                        React.DOM.button({ type: "button", className: "btn btn-primary", onClick: this.searchQuestions },
                                            React.DOM.span({ className: "glyphicon glyphicon-search" })
                                        )
                                    )
                                )
                            ),
                            React.DOM.div({ className: "col-md-12 margin-top-10", style: { "maxHeight": "530px", "overflowY": "auto" } },
                                React.DOM.table({ className: "table" },
                                    React.DOM.thead(null,
                                        React.DOM.tr(null,
                                            React.DOM.th({ className: "col-md-4" }, "名称"),
                                            React.DOM.th({ className: "col-md-1" }, "类型"),
                                            React.DOM.th({ className: "col-md-2" }, "标签"),
                                            React.DOM.th({ className: "col-md-2" }, "操作")
                                        )
                                    ),
                                    React.DOM.tbody(null, questionsDOM)
                                )
                            ),
                            ReactExtend.DOM.pagination({ className: "col-md-12", pageCount: pageCount, currentIndex: currentIndex, prev: true, next: true, flush: this.pageQuestions })
                        )
                    );
                }
            });
            ReactDOM.render(React.createElement(questionTableClass), document.querySelector("#templateTable"));
        }
    }
    baseOp.init_Objects();
    baseOp.init_PaperTable();
})();