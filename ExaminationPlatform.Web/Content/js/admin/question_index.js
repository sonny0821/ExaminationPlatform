(function () {
    var baseOp = {
        //数据源及操作方法
        init_Objects: function () {
            this.pools = {
                selectedPoolIds: new Array(),
                search: function (filter, callback) {
                    $.ajax({
                        url: "/api/QuestionPool",
                        type: "GET",
                        data: filter,
                        dataType: "json",
                        success: function (result) {
                            if (result.IsSuccess) {
                                if (callback instanceof Function) {
                                    callback(result.Data);
                                }
                            }
                        }
                    });
                },
                add: function (name, callback) {
                    if (!name) {
                        alert("新题库名称不能为空");
                        return;
                    }
                    $.ajax({
                        url: "/api/QuestionPool",
                        type: "POST",
                        data: { Name: name },
                        dataType: "json",
                        success: function (result) {
                            if (result.IsSuccess) {
                                if (callback instanceof Function) {
                                    callback(result.Data);
                                }
                            }
                        }
                    });
                },
                edit: function (pool, callback) {
                    $.ajax({
                        url: "/api/QuestionPool",
                        type: "PUT",
                        data: { Id: pool.Id, Name: pool.Name, Tag: pool.Tag, Summary: pool.Summary },
                        dataType: "json",
                        success: function (result) {
                            if (result.IsSuccess) {
                                if (callback instanceof Function) {
                                    callback(result.Data);
                                }
                            }
                            else {
                                cop.showBox({
                                    title: "提示",
                                    content: "保存被编辑的信息出错",
                                    ok: {
                                        label: "确定"
                                    }
                                })
                            }
                        }
                    });
                },
                remove: function (id, callback) {
                    $.ajax({
                        url: "/api/QuestionPool?id=" + id,
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
            };
            this.questions = {
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
        //题库表
        init_PoolsList: function () {
            var poolListClass = React.createClass({
                getInitialState: function () {
                    return { source: null };
                },
                componentWillMount: function () {
                    var that = this,
                        filter = { name: "" };
                    baseOp.pools.search(filter, function (data) {
                        that.setState({ source: data });
                    })
                },
                searchPools: function () {
                    var that = this,
                        filter = { name: that.refs.searchPoolName.value };
                    baseOp.pools.search(filter, function (data) {
                        that.setState({ source: data });
                    })
                },
                selectPools: function (poolId, isChecked) {
                    var that = this;
                    if (isChecked) {
                        baseOp.pools.selectedPoolIds.push(poolId);
                    }
                    else {
                        var index = baseOp.pools.selectedPoolIds.indexOf(poolId);
                        baseOp.pools.selectedPoolIds.splice(index, 1);
                    }
                },
                addPool: function () {
                    var that = this,
                        name = that.refs.addPoolName.value;
                    baseOp.pools.add(name, function (data) {
                        var list = that.state.source;
                        list.unshift(data);
                        that.setState({ source: list });
                        that.refs.addPoolName.value = "";
                    });
                },
                deletePool: function (index) {
                    var that = this,
                        pool = that.state.source[index];
                    if (confirm("确定要删除题库：" + pool.Name)) {
                        baseOp.pools.remove(pool.Id, function () {
                            var list = that.state.source;
                            list.splice(index, 1);
                            that.setState({ source: list });
                        });
                    }
                },
                render: function () {
                    var that = this,
                        poolClass = React.createClass({
                            getInitialState: function () {
                                return { source: this.props.source };
                            },
                            addQuestion: function () {
                                baseOp.questions.add(this.props.source.Id);
                            },
                            editPool: function () {
                                var poolEle = this,
                                    pool = {};
                                for (name in poolEle.state.source) {
                                    pool[name] = poolEle.state.source[name];
                                }
                                cop.showBox({
                                    type: "big",
                                    title: "题库编辑器",
                                    content: React.createElement(
                                        React.createClass({
                                            nameChange: function () {
                                                pool.Name = this.refs.name.value;
                                            },
                                            tagChange: function () {
                                                pool.Tag = this.refs.tag.value;
                                            },
                                            summaryChange: function () {
                                                pool.Summary = this.refs.summary.value;
                                            },
                                            render: function () {
                                                return React.DOM.div(null,
                                                    React.DOM.div({ className: "input-group margin-bottom-10" },
                                                        React.DOM.span({ className: "input-group-addon" }, "题库名称"),
                                                        React.DOM.input({ type: "text", className: "form-control", defaultValue: pool.Name, onChange: this.nameChange, ref: "name" })
                                                    ),
                                                    React.DOM.div({ className: "input-group margin-bottom-10" },
                                                        React.DOM.span({ className: "input-group-addon" }, "标签"),
                                                        React.DOM.input({ type: "text", className: "form-control", defaultValue: pool.Tag, onChange: this.tagChange, ref: "tag" })
                                                    ),
                                                    React.DOM.div({ className: "input-group margin-bottom-10" },
                                                        React.DOM.span({ className: "input-group-addon" }, "简介"),
                                                        React.DOM.textarea({ type: "text", className: "form-control", defaultValue: pool.Summary, onChange: this.summaryChange, ref: "summary" })
                                                    )
                                                )
                                            }
                                        })
                                    ),
                                    buttons: [{
                                        label: "保存",
                                        event: function () {
                                            baseOp.pools.edit(pool, function () {
                                                poolEle.setState({ source: pool });
                                            });
                                        }
                                    },
                                    {
                                        label: "取消"
                                    }]
                                });
                            },
                            deletePool: function () {
                                if (this.props.deletePool instanceof Function) {
                                    this.props.deletePool(this.props.index);
                                }
                            },
                            checkPool: function () {
                                var checkedId = this.state.source.Id;
                                this.props.selectPools(checkedId, this.refs.checkbox.checked);
                            },
                            render: function () {
                                return (
                                    React.DOM.li({ className: "list-group-item" },
                                        React.DOM.span({ className: "badge" }, this.state.source.QCount),
                                        React.DOM.div({ className: "page-header margin-top-0 margin-bottom-10" },
                                            React.DOM.input({ type: "checkbox", style: { "width": "10%" }, checked: this.state.source.checked, name: "pool", onChange: this.checkPool, ref: "checkbox" }),
                                            React.DOM.span({ style: { "display": "inline-block", "width": "80%", "cursor": "pointer" }, "data-toggle": "collapse", "data-target": "#pool" + this.props.index }, this.state.source.Name)
                                        ),
                                        React.DOM.div({ className: "collapse", id: "pool" + this.props.index },
                                            React.DOM.button({ className: "btn btn-success margin-left-10", type: "button", onClick: this.addQuestion },
                                                React.DOM.span({ className: "glyphicon glyphicon-plus" })
                                            ),
                                            React.DOM.button({ className: "btn btn-primary margin-left-10", onClick: this.editPool },
                                                React.DOM.span({ className: "glyphicon glyphicon-pencil" })
                                            ),
                                            React.DOM.button({ className: "btn btn-danger margin-left-10", onClick: this.deletePool },
                                                React.DOM.span({ className: "glyphicon glyphicon-remove" })
                                            )
                                        )
                                    )
                                );
                            }
                        });
                    if (this.state.source instanceof Array) {
                        var poolsDOM = this.state.source.map(function (pool, index) {
                            return React.createElement(poolClass, { source: pool, key: index, index: index, selectPools: that.selectPools, deletePool: that.deletePool });
                        });
                    }
                    return React.DOM.div(null,
                        React.DOM.div({ className: "input-group" },
                            React.DOM.input({ type: "text", className: "form-control", placeholder: "题库名称", onKeyUp: function (e) { e.keyCode == 13 ? that.searchPools() : null; }, ref: "searchPoolName" }),
                            React.DOM.span({ className: "input-group-btn" },
                                React.DOM.button({ className: "btn btn-primary", type: "button", onClick: this.searchPools },
                                    React.DOM.span({ className: "glyphicon glyphicon-search" })
                                )
                            )
                        ),
                        React.DOM.ul({ className: "list-group margin-top-10 margin-bottom-0", style: { "maxHeight": "550px", "overflowY": "auto" } }, poolsDOM),
                        React.DOM.div({ className: "input-group" },
                            React.DOM.input({ type: "text", className: "form-control", onKeyUp: function (e) { e.keyCode == 13 ? that.addPool() : null; }, ref: "addPoolName" }),
                            React.DOM.span({ className: "input-group-btn" },
                                React.DOM.button({ className: "btn btn-success", type: "button", onClick: this.addPool },
                                    React.DOM.span({ className: "glyphicon glyphicon-plus" })
                                )
                            )
                        )
                    );
                }
            });
            ReactDOM.render(React.createElement(poolListClass), document.querySelector("#poolList"));
        },
        //试题表
        init_QuestionTable: function () {
            var questionTableClass = React.createClass({
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
                                    React.DOM.td({ dangerouslySetInnerHTML: { __html: question.Content } }),
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
                                            React.DOM.th({ className: "col-md-4" }, "题干"),
                                            React.DOM.th({ className: "col-md-1" }, "类型"),
                                            React.DOM.th({ className: "col-md-2" }, "标签"),
                                            React.DOM.th({ className: "col-md-3" }, "题库"),
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
            ReactDOM.render(React.createElement(questionTableClass), document.querySelector("#questionTable"));
        }
    }
    baseOp.init_Objects();
    baseOp.init_PoolsList();
    baseOp.init_QuestionTable();
})();