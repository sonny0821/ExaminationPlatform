(function () {
    var baseOp =
    {
        init_Objects: function () {
            this.id = cop.getQueryStringByName("id");
            this.poolId = cop.getQueryStringByName("poolId");
            this.question = {
                data: null,
                init: function (callback) {
                    if (baseOp.id) {
                        $.ajax({
                            url: "/api/Question",
                            type: "GET",
                            dataType: "json",
                            data: { id: baseOp.id },
                            success: function (result) {
                                if (result.IsSuccess) {
                                    baseOp.question.data = result.Data;
                                    location.hash = result.Data.Type;
                                    if (callback instanceof Function) {
                                        callback();
                                    }
                                }
                            }
                        });
                    }
                },
                save: function (data, callback) {
                    var type;
                    if (baseOp.id) {
                        type = "PUT";
                    }
                    else {
                        type = "POST";
                    }
                    $.ajax({
                        url: "/api/Question",
                        type: type,
                        dataType: "json",
                        data: data,
                        success: function (result) {
                            if (result.IsSuccess) {
                                if (callback instanceof Function) {
                                    callback();
                                }
                                location.href = "/Admin/Question";
                            }
                            else {
                                cop.showBox({
                                    title: "提示",
                                    type: "small",
                                    content: "保存出错",
                                    buttons: [{ label: "确定" }]
                                });
                            }
                        }
                    });
                }
            };
            if (this.id) {
                baseOp.question.init(baseOp.init_QuestionType);
            } else {
                var currentType = parseInt(location.hash.split("#")[1]);
                baseOp.question.data = { PoolId: baseOp.poolId, Type: currentType, Options: [{ Content: "" }] };
                baseOp.init_QuestionType();
            }
        },
        init_QuestionEditor: function (data) {
            var type = parseInt(location.hash.split("#")[1]);
            type = type ? type : 0;
            switch (type) {
                case 1:
                    type = "radio";
                    break;
                case 2:
                    type = "multi";
                    break;
                case 3:
                    type = "subject";
                    break;
                case 4:
                    type = "score";
                    break;
                case 5:
                    type = "cumulation";
                    break;
                case 6:
                    type = "statistics";
                    break;
                default:
                    type = "invalid";
                    break;
            }
            if (type == "invalid") {
                ReactDOM.render(React.DOM.div({ style: { borderRadius: "10px", border: "5px solid #d3d3d3", textAlign: "center", margin: "30px 20%", width: "50%", minHeight: "300px" } },
                                React.DOM.h1({ style: { color: "#aaa" } }, "让我们开始创建", React.createElement("br"), "试题吧"),
                                React.DOM.span({ className: "glyphicon glyphicon-hand-left" })
                            ), document.querySelector("#questionEditor"));
            }
            else {
                ReactDOM.render(ReactExtend.DOM.questionEditor({ source: data, type: type, save: baseOp.question.save }), document.querySelector("#questionEditor"));
            }
        },
        init_QuestionType: function () {
            $.ajax({
                url: "/api/QuestionType",
                type: "GET",
                dataType: "json",
                success: function (result) {
                    if (result.IsSuccess) {
                        var typesListClass = React.createClass({
                            getInitialState: function () {
                                var currentType = parseInt(location.hash.split("#")[1]);
                                return { source: this.props.source, currentType: currentType };
                            },
                            componentWillMount: function () {
                                var currentType = this.state.currentType,
                                    source = this.state.source;
                                if (currentType) {
                                    source.filter(function (t) { return t.Value == currentType })[0].IsSelected = true;
                                    baseOp.question.data.Type = currentType;
                                }
                                baseOp.init_QuestionEditor(baseOp.question.data);
                            },
                            render: function () {
                                var that = this,
                                    currentType = this.state.currentType,
                                    source = this.state.source,
                                    typeClass = React.createClass({
                                        selected: function () {
                                            var source = this.props.source,
                                                selectFunc = this.props.selected;
                                            if (source.Value != currentType) {
                                                if (baseOp.id) {
                                                    cop.showBox({
                                                        title: "提示",
                                                        content: "更改题型后，部分数据将不会保存，是否继续更改",
                                                        type: "small",
                                                        buttons: [
                                                            { label: "确定", event: function () { selectFunc(source); } },
                                                            { label: "取消" }
                                                        ]
                                                    })
                                                }
                                                else {
                                                    this.props.selected(this.props.source);
                                                }
                                            }
                                        },
                                        render: function () {
                                            return React.DOM.a({ href: "javascript:void(0)", className: "list-group-item" + (this.props.source.IsSelected ? " active" : ""), onClick: this.selected }, this.props.source.Name);
                                        }
                                    }),
                                    typesDOM = source.map(function (type, index) {
                                        return (
                                            React.createElement(typeClass, {
                                                source: type,
                                                key: index,
                                                selected: function (t) {
                                                    source.forEach(function (el) {
                                                        el.IsSelected = false;
                                                    });
                                                    t.IsSelected = true;
                                                    that.setState({ source: source, currentType: t.Value });
                                                    location.hash = t.Value;
                                                    baseOp.question.data.Type = t.Value;
                                                    baseOp.question.data.Options = [{ Content: "" }];
                                                    baseOp.init_QuestionEditor(baseOp.question.data);
                                                }
                                            }));
                                    });
                                return React.DOM.div({ className: "list-group", style: { "maxHeight": "600px", "overflowY": "auto" } }, typesDOM);
                            }
                        });
                        ReactDOM.render(React.createElement(typesListClass, { source: result.Data }), document.querySelector("#typesList"));
                    }
                }
            });
        }
    };
    baseOp.init_Objects();
})();