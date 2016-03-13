(function () {
    var _extendLib = {
        "button": React.createClass({
            render: function () {
                var type = this.props.type,
                    className = this.props.className,
                    onClick = this.props.onClick,
                    content = this.props.content;
                return React.DOM.button({ className: "btn btn-" + (type ? type : "default") + (className ? " " + className : ""), onClick: onClick }, content);
            }
        }),
        "pagination": React.createClass({
            getInitialState: function () {
                return { currentIndex: this.props.currentIndex ? this.props.currentIndex : 1 };
            },
            selectPage: function (type, index) {
                var flush = this.props.flush;
                this.setState({ currentIndex: index }, function () {
                    if (flush instanceof Function) {
                        flush(index);
                    }
                });
            },
            render: function () {
                var com = this,
                    pageCount = com.props.pageCount,
                    index = com.state.index,
                    pages = new Array(),
                    page = React.createClass({
                        selectPage: function () {
                            var type = this.props.type,
                                index = this.props.index;
                            this.props.selectPage(type, index);
                        },
                        render: function () {
                            var pageClassName,
                                currentIndex = this.props.currentIndex,
                                type = this.props.type,
                                index = this.props.index,
                                content = this.props.content;
                            if ((type == "prev" || type == "next") && index == currentIndex) {
                                pageClassName = "disabled";
                            }
                            else if (type == "index" && this.props.currentIndex == index) {
                                pageClassName = "active";
                            }
                            return React.DOM.li({ className: pageClassName }, React.DOM.a({ href: "#", onClick: this.selectPage }, content));
                        }
                    });
                if (pageCount > 0) {
                    for (var i = 0; i < pageCount; i++) {
                        pages.push(React.createElement(page, {
                            key: i,
                            content: i + 1,
                            index: i + 1,
                            type: "index",
                            currentIndex: this.state.currentIndex,
                            selectPage: this.selectPage
                        }));
                    }
                    if (this.props.prev) {
                        pages.unshift(React.createElement(page, {
                            key: -2,
                            content: "<",
                            index: 1,
                            type: "prev",
                            currentIndex: this.state.currentIndex,
                            selectPage: this.selectPage
                        }));
                    }
                    if (this.props.next) {
                        pages.push(React.createElement(page, {
                            key: -1,
                            content: ">",
                            index: pageCount,
                            type: "next",
                            currentIndex: this.state.currentIndex,
                            selectPage: this.selectPage
                        }));
                    }
                }
                return React.DOM.div({ className: com.props.className }, React.DOM.nav(null, React.DOM.ul({ className: "pagination" }, pages)));
            }
        }),
        "alloyEditor": React.createClass({
            componentDidMount: function () {
                if (AlloyEditor) {
                    var alloyEditorConfig = {
                        toolbars: {
                            add: {
                                buttons: ['image', 'hline', 'table']
                            },
                            styles: {
                                selections: [
                                    {
                                        name: 'text',
                                        buttons: [{
                                            name: 'styles',
                                            cfg: {
                                                styles: [
                                                    {
                                                        name: 'Head 1',
                                                        style: { element: 'h1' }
                                                    },
                                                    {
                                                        name: 'Head 2',
                                                        style: { element: 'h2' }
                                                    },
                                                    {
                                                        name: 'Big',
                                                        style: { element: 'big' }
                                                    },
                                                    {
                                                        name: 'Small',
                                                        style: { element: 'small' }
                                                    },
                                                    {
                                                        name: 'Code',
                                                        style: { element: 'code' }
                                                    }
                                                ]
                                            }
                                        }, 'bold', 'italic', 'underline', 'ul', 'ol', 'paragraphLeft', 'paragraphRight', 'paragraphCenter', 'paragraphJustify', 'removeFormat'
                                        ],
                                        test: AlloyEditor.SelectionTest.text
                                    },
                                    {
                                        name: 'image',
                                        buttons: ['imageLeft', 'imageCenter', 'imageRight'],
                                        test: AlloyEditor.SelectionTest.image
                                    },
                                    {
                                        name: 'table',
                                        buttons: ['tableCell', 'tableColumn', 'tableHeading', 'tableRemove', 'tableRow'],
                                        test: AlloyEditor.SelectionTest.table
                                    }
                                ]
                            }
                        }
                    };
                    for (name in this.props.alloyEditorConfig) {
                        alloyEditorConfigp[Name] = this.props.alloyEditorConfig[name];
                    }
                    this._editor = AlloyEditor.editable(this.props.container, alloyEditorConfig);
                }
            },
            componentWillUnmount: function () {
                this._editor.destroy();
            },
            contentChanged: function () {
                this.props.onBlur(this._editor.get('nativeEditor').getData());
            },
            render: function () {
                var style = { background: "#fff", overflow: "auto" };
                for (name in this.props.style) {
                    style[name] = this.props.style[name];
                }
                return React.DOM.div({ className: "form-control ae-placeholder", contentEditable: "true", "data-placeholder": this.props.placeholder, style: style, id: this.props.container, dangerouslySetInnerHTML: { __html: this.props.defaultValue }, onBlur: this.contentChanged });
            }
        })
    };
    (function (w) {
        var _extend = function () {
            this.DOM = {};
            for (name in _extendLib) {
                this.DOM[name] = React.createElement.bind(null, _extendLib[name]);
            }
        };
        _extend.prototype = {
            extend: function (components) {
                for (name in components) {
                    this.DOM[name] = React.createElement.bind(null, components[name]);
                }
            }
        };
        w.ReactExtend = new _extend();
    })(window);
})();