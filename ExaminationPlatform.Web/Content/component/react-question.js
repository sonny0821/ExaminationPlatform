(function () {
    function getOptionClass(type) {
        switch (type) {
            case "radio":
            case "multi":
                return (
                    React.createClass({
                        getInitialState: function () {
                            return { source: this.props.source }
                        },
                        upOption: function () {
                            this.props.moveOption(this.state.source, -1);
                        },
                        downOption: function () {
                            this.props.moveOption(this.state.source, 1);
                        },
                        deleteOption: function () {
                            if (this.props.deleteFunc instanceof Function) {
                                this.props.deleteFunc(this.props.index);
                            }
                        },
                        contentChange: function (content) {
                            this.state.source.Content = content;
                        },
                        answerChange: function () {
                            this.state.source.Answer = this.refs.answer.checked;
                        },
                        textChange: function () {
                            this.state.source.HasText = this.refs.text.checked;
                        },
                        render: function () {
                            var buttons = [];
                            if (this.props.index > 0) {
                                buttons.push(React.DOM.button({ className: "btn btn-success margin-right-10", key: 1, onClick: this.upOption },
                                    React.DOM.span({ className: "glyphicon glyphicon-arrow-up" })
                                ));
                            }
                            if (this.props.index < this.props.length - 1) {
                                buttons.push(React.DOM.button({ className: "btn btn-success margin-right-10", key: 2, onClick: this.downOption },
                                    React.DOM.span({ className: "glyphicon glyphicon-arrow-down" })
                                ));
                            }
                            if (this.props.length > 1) {
                                buttons.push(React.DOM.button({ className: "btn btn-danger", key: 3, onClick: this.deleteOption },
                                    React.DOM.span({ className: "glyphicon glyphicon-remove" })
                                ));
                            }
                            return (
                                React.DOM.li({ className: "list-group-item" },
                                    React.DOM.div({ className: "row" },
                                        React.DOM.div({ className: "col-md-8" },
                                            React.DOM.div({ className: "input-group" },
                                                React.DOM.span({ className: "input-group-addon" }, "选项"),
                                                ReactExtend.DOM.alloyEditor({ container: "option" + this.props.index, defaultValue: this.state.source.Content, alloyEditorConfig: {}, style: { height: "70px" }, onBlur: this.contentChange })
                                            )
                                        ),
                                        React.DOM.div({ className: "col-md-2" },
                                            React.DOM.div(null,
                                                React.DOM.span(null, "答案"),
                                                React.DOM.input({ type: "checkbox", defaultChecked: this.state.source.Answer, onChange: this.answerChange, ref: "answer" })
                                            ),
                                            React.DOM.div(null,
                                                React.DOM.span(null, "附加"),
                                                React.DOM.input({ type: "checkbox", defaultChecked: this.state.source.HasText, onChange: this.textChange, ref: "text" })
                                            )
                                        ),
                                        React.DOM.div({ className: "col-md-2" }, buttons)
                                    )
                                )
                            );
                        }
                    }));
            case "score":
                return (
                    React.createClass({
                        getInitialState: function () {
                            var minValue = 1,
                                maxValue = 5;
                            if (this.props.source.Content) {
                                var values = this.props.source.Content.split(":");
                                var minValue = parseInt(values[0]),
                                    maxValue = parseInt(values[1]);
                            }
                            else {
                                this.props.source.Content = minValue + ":" + maxValue;
                            }
                            return { source: this.props.source, minValue: minValue, maxValue: maxValue }
                        },
                        maxValueChange: function () {
                            var source = this.state.source;
                            source.Content = this.refs.minValue.value + ":" + this.refs.maxValue.value;
                            this.setState({ source: source })
                        },
                        render: function () {
                            var buttons = [];
                            return React.DOM.li({ className: "list-group-item" },
                                React.DOM.div({ className: "row" },
                                    React.DOM.div({ className: "col-md-10" },
                                        React.DOM.div({ className: "col-md-3" },
                                            React.DOM.div({ className: "input-group" },
                                                React.DOM.span({ className: "input-group-addon" }, "最小值"),
                                                React.DOM.input({ className: "form-control", type: "number", defaultValue: this.state.minValue, readOnly: true, ref: "minValue" })
                                            )
                                        ),
                                        React.DOM.div({ className: "col-md-3" },
                                            React.DOM.div({ className: "input-group" },
                                                React.DOM.span({ className: "input-group-addon" }, "最大值"),
                                                React.DOM.input({ className: "form-control", type: "number", defaultValue: this.state.maxValue, min: this.state.minValue + 2, max: this.state.minValue + 9, onBlur: this.maxValueChange, ref: "maxValue" })
                                            )
                                        )
                                    ),
                                    React.DOM.div({ className: "col-md-2" }, buttons)
                                )
                            );
                        }
                    }));
            case "cumulation":
                return (
                    React.createClass({
                        getInitialState: function () {
                            return { source: this.props.source }
                        },
                        upOption: function () {
                            this.props.moveOption(this.state.source, -1);
                        },
                        downOption: function () {
                            this.props.moveOption(this.state.source, 1);
                        },
                        deleteOption: function () {
                            if (this.props.deleteFunc instanceof Function) {
                                this.props.deleteFunc(this.props.index);
                            }
                        },
                        contentChange: function (content) {
                            this.state.source.Content = content;
                        },
                        tagChange: function () {
                            this.state.source.Tag = this.refs.tag.value;
                        },
                        textChange: function () {
                            this.state.source.HasText = this.refs.text.checked;
                        },
                        render: function () {
                            var buttons = [];
                            if (this.props.index > 0) {
                                buttons.push(React.DOM.button({ className: "btn btn-success margin-right-10", key: 1, onClick: this.upOption },
                                    React.DOM.span({ className: "glyphicon glyphicon-arrow-up" })
                                ));
                            }
                            if (this.props.index < this.props.length - 1) {
                                buttons.push(React.DOM.button({ className: "btn btn-success margin-right-10", key: 2, onClick: this.downOption },
                                    React.DOM.span({ className: "glyphicon glyphicon-arrow-down" })
                                ));
                            }
                            if (this.props.length > 1) {
                                buttons.push(React.DOM.button({ className: "btn btn-danger", key: 3, onClick: this.deleteOption },
                                    React.DOM.span({ className: "glyphicon glyphicon-remove" })
                                ));
                            }
                            return React.DOM.li({ className: "list-group-item" },
                                React.DOM.div({ className: "row" },
                                    React.DOM.div({ className: "col-md-7" },
                                        React.DOM.div({ className: "input-group" },
                                            React.DOM.span({ className: "input-group-addon" }, "选项"),
                                            ReactExtend.DOM.alloyEditor({ container: "option" + this.props.index, defaultValue: this.state.source.Content, alloyEditorConfig: {}, style: { height: "70px" }, onBlur: this.contentChange })
                                        )
                                    ),
                                    React.DOM.div({ className: "col-md-2" },
                                        React.DOM.div({ className: "input-group" },
                                            React.DOM.span({ className: "input-group-addon" }, "分数"),
                                            React.DOM.input({ className: "form-control", type: "number", min: 1, defaultValue: this.state.source.Tag, onBlur: this.tagChange, ref: "tag" })
                                        )
                                    ),
                                    React.DOM.div({ className: "col-md-1" },
                                        React.DOM.div(null,
                                            React.DOM.span(null, "附加"),
                                            React.DOM.input({ type: "checkbox", defaultChecked: this.state.source.HasText, onChange: this.textChange, ref: "text" })
                                        )
                                    ),
                                    React.DOM.div({ className: "col-md-2" }, buttons)
                                )
                            );
                        }
                    }));
            case "statistics":
                return (
                    React.createClass({
                        getInitialState: function () {
                            return { source: this.props.source }
                        },
                        upOption: function () {
                            this.props.moveOption(this.state.source, -1);
                        },
                        downOption: function () {
                            this.props.moveOption(this.state.source, 1);
                        },
                        deleteOption: function () {
                            if (this.props.deleteFunc instanceof Function) {
                                this.props.deleteFunc(this.props.index);
                            }
                        },
                        contentChange: function (content) {
                            this.state.source.Content = content;
                        },
                        tagChange: function () {
                            this.state.source.Tag = this.refs.tag.value;
                        },
                        textChange: function () {
                            this.state.source.HasText = this.refs.text.checked;
                        },
                        render: function () {
                            var buttons = [];
                            if (this.props.index > 0) {
                                buttons.push(React.DOM.button({ className: "btn btn-success margin-right-10", key: 1, onClick: this.upOption },
                                    React.DOM.span({ className: "glyphicon glyphicon-arrow-up" })
                                ));
                            }
                            if (this.props.index < this.props.length - 1) {
                                buttons.push(React.DOM.button({ className: "btn btn-success margin-right-10", key: 2, onClick: this.downOption },
                                    React.DOM.span({ className: "glyphicon glyphicon-arrow-down" })
                                ));
                            }
                            if (this.props.length > 1) {
                                buttons.push(React.DOM.button({ className: "btn btn-danger", key: 3, onClick: this.deleteOption },
                                    React.DOM.span({ className: "glyphicon glyphicon-remove" })
                                ));
                            }
                            return React.DOM.li({ className: "list-group-item" },
                                React.DOM.div({ className: "row" },
                                    React.DOM.div({ className: "col-md-7" },
                                        React.DOM.div({ className: "input-group" },
                                            React.DOM.span({ className: "input-group-addon" }, "选项"),
                                            ReactExtend.DOM.alloyEditor({ container: "option" + this.props.index, defaultValue: this.state.source.Content, alloyEditorConfig: {}, style: { height: "70px" }, onBlur: this.contentChange })
                                        )
                                    ),
                                    React.DOM.div({ className: "col-md-2" },
                                        React.DOM.div({ className: "input-group" },
                                            React.DOM.span({ className: "input-group-addon" }, "表示项"),
                                            React.DOM.input({ className: "form-control", defaultValue: this.state.source.Tag, onBlur: this.tagChange, ref: "tag" })
                                        )
                                    ),
                                    React.DOM.div({ className: "col-md-1" },
                                        React.DOM.div(null,
                                            React.DOM.span(null, "附加"),
                                            React.DOM.input({ type: "checkbox", defaultChecked: this.state.source.HasText, onChange: this.textChange, ref: "text" })
                                        )
                                    ),
                                    React.DOM.div({ className: "col-md-2" }, buttons)
                                )
                            );
                        }
                    }));
            default:
                break;
        }
    }
    function getQuestionClass(type) {
        switch (type) {
            case "radio":
            case "multi":
                return (
                    React.createClass({
                        getInitialState: function () {
                            return { source: this.props.source }
                        },
                        addOption: function () {
                            var options = this.state.source.Options;
                            options.push({ Content: "" });
                            this.setState({ source: { Options: options } });
                        },
                        deleteOption: function (index) {
                            var options = this.state.source.Options;
                            options.splice(index, 1);
                            this.setState({ source: { Options: options } });
                        },
                        moveOption: function (option, behavior) {
                            var options = this.state.source.Options;
                            var index = options.indexOf(option);
                            options.splice(index, 1);
                            options.splice(index + behavior, 0, option);
                            this.setState({ source: { Options: options } });
                        },
                        contentChange: function (content) {
                            this.state.source.Content = content;
                        },
                        tagChange: function () {
                            this.state.source.Tag = this.refs.tag.value;
                        },
                        render: function () {
                            var instance = this,
                                optionEditorClass = getOptionClass(type),
                                optionEditors = instance.state.source.Options.map(function (option, index) {
                                    return React.createElement(optionEditorClass, { source: option, key: index, index: index, length: instance.state.source.Options.length, deleteFunc: instance.deleteOption, moveOption: instance.moveOption });
                                });
                            return React.DOM.div({ className: "row" },
                                React.DOM.div({ className: "col-md-12 margin-bottom-10" },
                                    React.DOM.div({ className: "input-group" },
                                        React.DOM.span({ className: "input-group-addon" }, "题干"),
                                        ReactExtend.DOM.alloyEditor({ container: "question", defaultValue: this.state.source.Content, alloyEditorConfig: {}, style: { height: "250px" }, placeholder: "题干...", onBlur: instance.contentChange })
                                    )
                                ),
                                React.DOM.div({ className: "col-md-12 margin-bottom-10" },
                                    React.DOM.div({ className: "input-group" },
                                        React.DOM.span({ className: "input-group-addon" }, "标签"),
                                        React.DOM.input({ className: "form-control", type: "text", defaultValue: this.state.source.Tag, ref: "tag", onBlur: instance.tagChange })
                                    )
                                ),
                                React.DOM.div({ className: "col-md-12 margin-bottom-10" },
                                    React.DOM.button({ className: "btn btn-primary", onClick: this.addOption }, "添加选项")
                                ),
                                React.DOM.div({ className: "col-md-12 margin-bottom-10" },
                                    React.DOM.ul({ className: "list-group", style: { maxHeight: "600px", overflowY: "auto" } }, optionEditors)
                                )
                            );
                        }
                    })
                );
            case "subject":
                return React.createClass({
                    getInitialState: function () {
                        return { source: this.props.source }
                    },
                    contentChange: function (content) {
                        this.state.source.Content = content;
                    },
                    tagChange: function () {
                        this.state.source.Tag = this.refs.tag.value;
                    },
                    render: function () {
                        var instance = this;
                        return React.DOM.div({ className: "row" },
                            React.DOM.div({ className: "col-md-12 margin-bottom-10" },
                                React.DOM.div({ className: "input-group" },
                                    React.DOM.span({ className: "input-group-addon" }, "题干"),
                                    ReactExtend.DOM.alloyEditor({ container: "question", defaultValue: this.state.source.Content, alloyEditorConfig: {}, style: { height: "250px" }, placeholder: "题干...", onBlur: instance.contentChange })
                                )
                            ),
                            React.DOM.div({ className: "col-md-12 margin-bottom-10" },
                                React.DOM.div({ className: "input-group" },
                                    React.DOM.span({ className: "input-group-addon" }, "标签"),
                                    React.DOM.input({ className: "form-control", type: "text", defaultValue: this.state.source.Tag, ref: "tag", onBlur: instance.tagChange })
                                )
                            )
                        );
                    }
                });
            case "score":
                return React.createClass({
                    getInitialState: function () {
                        return { source: this.props.source }
                    },
                    contentChange: function (content) {
                        this.state.source.Content = content;
                    },
                    tagChange: function () {
                        this.state.source.Tag = this.refs.tag.value;
                    },
                    render: function () {
                        var instance = this,
                            scoreEditorClass = getOptionClass(type),
                            scoreEditors = instance.state.source.Options.map(function (option, index) {
                                return React.createElement(scoreEditorClass, { source: option, key: index, index: index, length: instance.state.source.Options.length, deleteFunc: instance.deleteOption });
                            });
                        return React.DOM.div({ className: "row" },
                            React.DOM.div({ className: "col-md-12 margin-bottom-10" },
                                React.DOM.div({ className: "input-group" },
                                    React.DOM.span({ className: "input-group-addon" }, "题干"),
                                    ReactExtend.DOM.alloyEditor({ container: "question", defaultValue: this.state.source.Content, alloyEditorConfig: {}, style: { height: "250px" }, placeholder: "题干...", onBlur: instance.contentChange })
                                )
                            ),
                            React.DOM.div({ className: "col-md-12 margin-bottom-10" },
                                React.DOM.div({ className: "input-group" },
                                    React.DOM.span({ className: "input-group-addon" }, "标签"),
                                    React.DOM.input({ className: "form-control", type: "text", defaultValue: this.state.source.Tag, ref: "tag", onBlur: instance.tagChange })
                                )
                            ),
                            React.DOM.div({ className: "col-md-12 margin-bottom-10" },
                                React.DOM.ul({ className: "list-group", style: { maxHeight: "300px", overflowY: "auto" } }, scoreEditors)
                            )
                        );
                    }
                });
            case "cumulation":
                return React.createClass({
                    getInitialState: function () {
                        return { source: this.props.source }
                    },
                    addOption: function () {
                        var options = this.state.source.Options;
                        options.push({});
                        this.setState({ source: { Options: options } });
                    },
                    deleteOption: function (index) {
                        var options = this.state.source.Options;
                        options.splice(index, 1);
                        this.setState({ source: { Options: options } });
                    },
                    moveOption: function (option, behavior) {
                        var options = this.state.source.Options;
                        var index = options.indexOf(option);
                        options.splice(index, 1);
                        options.splice(index + behavior, 0, option);
                        this.setState({ source: { Options: options } });
                    },
                    contentChange: function (content) {
                        this.state.source.Content = content;
                    },
                    tagChange: function () {
                        this.state.source.Tag = this.refs.tag.value;
                    },
                    render: function () {
                        var instance = this,
                            optionEditorClass = getOptionClass(type),
                            optionEditors = instance.state.source.Options.map(function (option, index) {
                                return React.createElement(optionEditorClass, { source: option, key: index, index: index, length: instance.state.source.Options.length, deleteFunc: instance.deleteOption, moveOption: instance.moveOption });
                            });
                        return React.DOM.div({ className: "row" },
                            React.DOM.div({ className: "col-md-12 margin-bottom-10" },
                                React.DOM.div({ className: "input-group" },
                                    React.DOM.span({ className: "input-group-addon" }, "题干"),
                                    ReactExtend.DOM.alloyEditor({ container: "question", defaultValue: this.state.source.Content, alloyEditorConfig: {}, style: { height: "250px" }, placeholder: "题干...", onBlur: instance.contentChange })
                                )
                            ),
                            React.DOM.div({ className: "col-md-12 margin-bottom-10" },
                                React.DOM.div({ className: "input-group" },
                                    React.DOM.span({ className: "input-group-addon" }, "标签"),
                                    React.DOM.input({ className: "form-control", type: "text", defaultValue: this.state.source.Tag, ref: "tag", onBlur: instance.tagChange })
                                )
                            ),
                            React.DOM.div({ className: "col-md-12 margin-bottom-10" },
                                React.DOM.button({ className: "btn btn-primary", onClick: this.addOption }, "添加选项")
                            ),
                            React.DOM.div({ className: "col-md-12 margin-bottom-10" },
                                React.DOM.ul({ className: "list-group", style: { maxHeight: "300px", overflowY: "auto" } }, optionEditors)
                            )
                        );
                    }
                });
            case "statistics":
                return React.createClass({
                    getInitialState: function () {
                        return { source: this.props.source }
                    },
                    addOption: function () {
                        var options = this.state.source.Options;
                        options.push({});
                        this.setState({ source: { Options: options } });
                    },
                    deleteOption: function (index) {
                        var options = this.state.source.Options;
                        options.splice(index, 1);
                        this.setState({ source: { Options: options } });
                    },
                    moveOption: function (option, behavior) {
                        var options = this.state.source.Options;
                        var index = options.indexOf(option);
                        options.splice(index, 1);
                        options.splice(index + behavior, 0, option);
                        this.setState({ source: { Options: options } });
                    },
                    contentChange: function (content) {
                        this.state.source.Content = content;
                    },
                    tagChange: function () {
                        this.state.source.Tag = this.refs.tag.value;
                    },
                    render: function () {
                        var instance = this,
                            optionEditorClass = getOptionClass(type),
                            optionEditors = instance.state.source.Options.map(function (option, index) {
                                return React.createElement(optionEditorClass, { source: option, key: index, index: index, length: instance.state.source.Options.length, deleteFunc: instance.deleteOption, moveOption: instance.moveOption });
                            });
                        return React.DOM.div({ className: "row" },
                            React.DOM.div({ className: "col-md-12 margin-bottom-10" },
                                React.DOM.div({ className: "input-group" },
                                    React.DOM.span({ className: "input-group-addon" }, "题干"),
                                    ReactExtend.DOM.alloyEditor({ container: "question", defaultValue: this.state.source.Content, alloyEditorConfig: {}, style: { height: "250px" }, placeholder: "题干...", onBlur: instance.contentChange })
                                )
                            ),
                            React.DOM.div({ className: "col-md-12 margin-bottom-10" },
                                React.DOM.div({ className: "input-group" },
                                    React.DOM.span({ className: "input-group-addon" }, "标签"),
                                    React.DOM.input({ className: "form-control", type: "text", defaultValue: this.state.source.Tag, ref: "tag", onBlur: instance.tagChange })
                                )
                            ),
                            React.DOM.div({ className: "col-md-12 margin-bottom-10" },
                                React.DOM.button({ className: "btn btn-primary", onClick: this.addOption }, "添加选项")
                            ),
                            React.DOM.div({ className: "col-md-12 margin-bottom-10" },
                                React.DOM.ul({ className: "list-group", style: { maxHeight: "300px", overflowY: "auto" } }, optionEditors)
                            )
                        );
                    }
                });
            default:
                break;
        }
    };
    ReactExtend.extend({
        questionEditor: React.createClass({
            getInitialState: function () {
                return { source: this.props.source }
            },
            saveQuestion: function () {
                var question = this.state.source;
                if (this.props.validate instanceof Function) {
                    if (this.props.validate()) {
                        this.props.save(question);
                    }
                }
                else {
                    this.props.save(question);
                }
            },
            render: function () {
                return React.DOM.div(null,
                        React.createElement(getQuestionClass(this.props.type), { source: this.props.source }),
                        React.DOM.div({ className: "row" },
                            React.DOM.div({ className: "col-md-12" },
                                React.DOM.button({ className: "btn btn-primary margin-right-15", onClick: this.saveQuestion }, "保存"),
                                React.DOM.button({ className: "btn btn-default" }, "取消")
                            )
                        )
                    );
            }
        })
    });
})();