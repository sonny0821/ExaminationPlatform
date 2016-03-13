
var cop = function () {
    function _getQueryStringByName(name) {
        var result = location.search.match(new RegExp("[\?\&]" + name + "=([^\&]+)", "i"));
        if (result == null || result.length < 1) {
            return "";
        }
        return result[1];
    };

    function _showBox(context) {
        var modalClass = React.createClass({
            componentDidMount: function () {
                $("#myModal").modal();
            },
            render: function () {
                var that = this,
                    typeStyle = "";
                if (that.props.source.type == "small") {
                    typeStyle = " modal-sm";
                }

                var buttons = that.props.source.buttons.map(function (button, index) {
                    return React.DOM.button({ type: "button", className: "btn btn-primary", name: "btnModalOk", "data-dismiss": "modal", onClick: button.event, key: index }, button.label);
                });
                return React.DOM.div({ className: "modal fade", id: "myModal", tabIndex: "-1", role: "dialog", "aria-labelledby": "myModalLabel" },
                    React.DOM.div({ className: "modal-dialog" + typeStyle, role: "document" },
                        React.DOM.div({ className: "modal-content" },
                            React.DOM.div({ className: "modal-header" },
                                React.DOM.button({ type: "button", className: "close", "data-dismiss": "modal", "aria-label": "Close" },
                                    React.DOM.span({ "aria-hidden": "true" }, "×")
                                ),
                                React.DOM.h4({ className: "modal-title" }, that.props.source.title)
                            ),
                            React.DOM.div({ className: "modal-body" }, that.props.source.content),
                            React.DOM.div({ className: "modal-footer" }, buttons)
                        )
                    )
                );
            }
        });
        ReactDOM.render(React.createElement(modalClass, { source: context }), document.querySelector("#modalContainer"));
    };

    return {
        getQueryStringByName: _getQueryStringByName,
        showBox: _showBox
    }
}();

Array.prototype.arrayRemove = function (obj) {
    var index = this.indexOf(obj);
    if (index > -1) {
        this.splice(index, 1);
    }
};

Array.prototype.toTop = function (obj) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == obj && i != 0) {
            this.arrayRemove(obj);
            this.unshift(obj);
            break;
        }
    }
};

Array.prototype.toLast = function (obj) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == obj && i != this.length - 1) {
            this.arrayRemove(obj);
            this.push(obj);
            break;
        }
    }
};

Array.prototype.toPrevious = function (obj) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == obj && i != 0) {
            this[i] = this[i - 1];
            this[i - 1] = obj;
            break;
        }
    }
};

Array.prototype.toNext = function (obj) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == obj && i != this.length - 1) {
            this[i] = this[i + 1];
            this[i + 1] = obj;
            break;
        }
    }
};