!function (e, t, o, s) {
    "use strict";
    let i = {
        settings: {
            injectStyle: !0,
            levels: 2,
            expandIcon: "fa fa-chevron-right",
            collapseIcon: "fa fa-chevron-down",
            emptyIcon: "fa",
            nodeIcon: "",
            selectedIcon: "",
            checkedIcon: "fa fa-check-circle-o",
            uncheckedIcon: "fa fa-circle-thin",
            color: void 0,
            backColor: void 0,
            borderColor: void 0,
            onhoverColor: "#F5F5F5",
            selectedColor: "#FFFFFF",
            selectedBackColor: "#428bca",
            searchResultColor: "#D9534F",
            searchResultBackColor: void 0,
            enableLinks: !1,
            enableIndent: !0,
            highlightSelected: !0,
            highlightSearchResults: !0,
            showBorder: !0,
            showIcon: !0,
            showCheckbox: !1,
            showTags: !1,
            multiSelect: !1,
            onNodeChecked: void 0,
            onNodeCollapsed: void 0,
            onNodeDisabled: void 0,
            onNodeEnabled: void 0,
            onNodeExpanded: void 0,
            onNodeSelected: void 0,
            onNodeUnchecked: void 0,
            onNodeUnselected: void 0,
            onSearchComplete: void 0,
            onSearchCleared: void 0
        }, options: {silent: !1, ignoreChildren: !1}, searchOptions: {ignoreCase: !0, exactMatch: !1, revealResults: !0}
    }, n = function (t, o) {
        return this.$element = e(t), this.elementId = t.id, this.styleId = this.elementId + "-style", this.init(o), {
            options: this.options,
            init: e.proxy(this.init, this),
            remove: e.proxy(this.remove, this),
            getNode: e.proxy(this.getNode, this),
            getParent: e.proxy(this.getParent, this),
            getSiblings: e.proxy(this.getSiblings, this),
            getSelected: e.proxy(this.getSelected, this),
            getUnselected: e.proxy(this.getUnselected, this),
            getExpanded: e.proxy(this.getExpanded, this),
            getCollapsed: e.proxy(this.getCollapsed, this),
            getChecked: e.proxy(this.getChecked, this),
            getUnchecked: e.proxy(this.getUnchecked, this),
            getDisabled: e.proxy(this.getDisabled, this),
            getEnabled: e.proxy(this.getEnabled, this),
            selectNode: e.proxy(this.selectNode, this),
            unselectNode: e.proxy(this.unselectNode, this),
            toggleNodeSelected: e.proxy(this.toggleNodeSelected, this),
            collapseAll: e.proxy(this.collapseAll, this),
            collapseNode: e.proxy(this.collapseNode, this),
            expandAll: e.proxy(this.expandAll, this),
            expandNode: e.proxy(this.expandNode, this),
            toggleNodeExpanded: e.proxy(this.toggleNodeExpanded, this),
            revealNode: e.proxy(this.revealNode, this),
            checkAll: e.proxy(this.checkAll, this),
            checkNode: e.proxy(this.checkNode, this),
            uncheckAll: e.proxy(this.uncheckAll, this),
            uncheckNode: e.proxy(this.uncheckNode, this),
            toggleNodeChecked: e.proxy(this.toggleNodeChecked, this),
            disableAll: e.proxy(this.disableAll, this),
            disableNode: e.proxy(this.disableNode, this),
            enableAll: e.proxy(this.enableAll, this),
            enableNode: e.proxy(this.enableNode, this),
            toggleNodeDisabled: e.proxy(this.toggleNodeDisabled, this),
            search: e.proxy(this.search, this),
            clearSearch: e.proxy(this.clearSearch, this)
        }
    };
    n.prototype.init = function (t) {
        this.tree = [], this.nodes = [], t.data && ("string" == typeof t.data && (t.data = e.parseJSON(t.data)), this.tree = e.extend(!0, [], t.data), delete t.data), this.options = e.extend({}, i.settings, t), this.destroy(), this.subscribeEvents(), this.setInitialStates({nodes: this.tree}, 0), this.render()
    }, n.prototype.remove = function () {
        this.destroy(), e.removeData(this, "treeview"), e("#" + this.styleId).remove()
    }, n.prototype.destroy = function () {
        this.initialized && (this.$wrapper.remove(), this.$wrapper = null, this.unsubscribeEvents(), this.initialized = !1)
    }, n.prototype.unsubscribeEvents = function () {
        this.$element.off("click"), this.$element.off("nodeChecked"), this.$element.off("nodeCollapsed"), this.$element.off("nodeDisabled"), this.$element.off("nodeEnabled"), this.$element.off("nodeExpanded"), this.$element.off("nodeSelected"), this.$element.off("nodeUnchecked"), this.$element.off("nodeUnselected"), this.$element.off("searchComplete"), this.$element.off("searchCleared")
    }, n.prototype.subscribeEvents = function () {
        this.unsubscribeEvents(), this.$element.on("click", e.proxy(this.clickHandler, this)), "function" == typeof this.options.onNodeChecked && this.$element.on("nodeChecked", this.options.onNodeChecked), "function" == typeof this.options.onNodeCollapsed && this.$element.on("nodeCollapsed", this.options.onNodeCollapsed), "function" == typeof this.options.onNodeDisabled && this.$element.on("nodeDisabled", this.options.onNodeDisabled), "function" == typeof this.options.onNodeEnabled && this.$element.on("nodeEnabled", this.options.onNodeEnabled), "function" == typeof this.options.onNodeExpanded && this.$element.on("nodeExpanded", this.options.onNodeExpanded), "function" == typeof this.options.onNodeSelected && this.$element.on("nodeSelected", this.options.onNodeSelected), "function" == typeof this.options.onNodeUnchecked && this.$element.on("nodeUnchecked", this.options.onNodeUnchecked), "function" == typeof this.options.onNodeUnselected && this.$element.on("nodeUnselected", this.options.onNodeUnselected), "function" == typeof this.options.onSearchComplete && this.$element.on("searchComplete", this.options.onSearchComplete), "function" == typeof this.options.onSearchCleared && this.$element.on("searchCleared", this.options.onSearchCleared)
    }, n.prototype.setInitialStates = function (t, o) {
        if (!t.nodes) return;
        o += 1;
        let s = t, i = this;
        e.each(t.nodes, function (e, t) {
            t.nodeId = i.nodes.length, t.parentId = s.nodeId, t.hasOwnProperty("selectable") || (t.selectable = !1), t.state = t.state || {}, t.state.hasOwnProperty("checked") || (t.state.checked = !1), t.state.hasOwnProperty("disabled") || (t.state.disabled = !1), t.state.hasOwnProperty("expanded") || (!t.state.disabled && o < i.options.levels && t.nodes && t.nodes.length > 0 ? t.state.expanded = !0 : t.state.expanded = !1), t.state.hasOwnProperty("selected") || (t.state.selected = !1), i.nodes.push(t), t.nodes && i.setInitialStates(t, o)
        })
    }, n.prototype.clickHandler = function (t) {
        this.options.enableLinks || t.preventDefault();
        let o = e(t.target), s = this.findNode(o);
        if (!s || s.state.disabled) return;
        let n = o.attr("class") ? o.attr("class").split(" ") : [];
        -1 !== n.indexOf("expand-icon") ? (this.toggleExpandedState(s, i.options), this.render()) : -1 !== n.indexOf("check-icon") ? (this.toggleCheckedState(s, i.options), this.render()) : (s.selectable ? this.toggleSelectedState(s, i.options) : this.toggleExpandedState(s, i.options), this.render())
    }, n.prototype.findNode = function (e) {
        let t = e.closest("li.list-group-item").attr("data-nodeid"), o = this.nodes[t];
        return o || console.log("Error: node does not exist"), o
    }, n.prototype.toggleExpandedState = function (e, t) {
        e && this.setExpandedState(e, !e.state.expanded, t)
    }, n.prototype.setExpandedState = function (t, o, s) {
        o !== t.state.expanded && (o && t.nodes ? (t.state.expanded = !0, s.silent || this.$element.trigger("nodeExpanded", e.extend(!0, {}, t))) : o || (t.state.expanded = !1, s.silent || this.$element.trigger("nodeCollapsed", e.extend(!0, {}, t)), t.nodes && !s.ignoreChildren && e.each(t.nodes, e.proxy(function (e, t) {
            this.setExpandedState(t, !1, s)
        }, this))))
    }, n.prototype.toggleSelectedState = function (e, t) {
        e && this.setSelectedState(e, !e.state.selected, t)
    }, n.prototype.setSelectedState = function (t, o, s) {
        o !== t.state.selected && (o ? (this.options.multiSelect || e.each(this.findNodes("true", "g", "state.selected"), e.proxy(function (e, t) {
            this.setSelectedState(t, !1, s)
        }, this)), t.state.selected = !0, s.silent || this.$element.trigger("nodeSelected", e.extend(!0, {}, t))) : (t.state.selected = !1, s.silent || this.$element.trigger("nodeUnselected", e.extend(!0, {}, t))))
    }, n.prototype.toggleCheckedState = function (e, t) {
        e && this.setCheckedState(e, !e.state.checked, t)
    }, n.prototype.setCheckedState = function (t, o, s) {
        o !== t.state.checked && (o ? (t.state.checked = !0, s.silent || this.$element.trigger("nodeChecked", e.extend(!0, {}, t))) : (t.state.checked = !1, s.silent || this.$element.trigger("nodeUnchecked", e.extend(!0, {}, t))))
    }, n.prototype.setDisabledState = function (t, o, s) {
        o !== t.state.disabled && (o ? (t.state.disabled = !0, this.setExpandedState(t, !1, s), this.setSelectedState(t, !1, s), this.setCheckedState(t, !1, s), s.silent || this.$element.trigger("nodeDisabled", e.extend(!0, {}, t))) : (t.state.disabled = !1, s.silent || this.$element.trigger("nodeEnabled", e.extend(!0, {}, t))))
    }, n.prototype.render = function () {
        this.initialized || (this.$element.addClass("treeview"), this.$wrapper = e(this.template.list), this.injectStyle(), this.initialized = !0), this.$element.empty().append(this.$wrapper.empty()), this.buildTree(this.tree, 0)
    }, n.prototype.buildTree = function (t, o) {
        if (!t) return;
        o += 1;
        let s = this;
        e.each(t, function (t, i) {
            let n = e(s.template.itemWrapper).addClass("node-" + s.elementId).addClass(i.state.checked ? "node-checked" : "").addClass(i.state.disabled ? "node-disabled" : "").addClass(i.state.selected ? "node-selected" : "").addClass(i.searchResult ? "search-result" : "").attr("data-nodeid", i.nodeId).attr("style", s.buildStyleOverride(i)),
                d = e(s.template.itemLeftElem), r = e(s.template.itemRightElem);
            if (n.append(d), n.append(r), s.options.enableIndent) for (let e = 0; e < o - 1; e++) d.append(s.template.indent);
            let l = [];
            if (i.nodes ? (l.push("expand-icon"), i.state.expanded ? l.push(s.options.collapseIcon) : l.push(s.options.expandIcon)) : l.push(s.options.emptyIcon), d.append(e(s.template.icon).addClass(l.join(" "))), s.options.showIcon) {
                let t = ["node-icon"];
                t.push(i.icon || s.options.nodeIcon), i.state.selected && (t.pop(), t.push(i.selectedIcon || s.options.selectedIcon || i.icon || s.options.nodeIcon)), d.append(e(s.template.icon).addClass(t.join(" ")))
            }
            if (s.options.showCheckbox) {
                let t = ["check-icon"];
                i.state.checked ? t.push(s.options.checkedIcon) : t.push(s.options.uncheckedIcon), d.append(e(s.template.icon).addClass(t.join(" ")))
            }
            if (s.options.enableLinks ? d.append(e(s.template.link).attr("href", i.href).append(i.text)) : d.append(i.text), s.options.showTags && i.tags && e.each(i.tags, function (t, o) {
                r.append(e(s.template.badge).append(o))
            }), s.$wrapper.append(n), i.nodes && i.state.expanded && !i.state.disabled) return s.buildTree(i.nodes, o)
        })
    }, n.prototype.buildStyleOverride = function (e) {
        if (e.state.disabled) return "";
        let t = e.color, o = e.backColor;
        return this.options.highlightSelected && e.state.selected && (this.options.selectedColor && (t = this.options.selectedColor), this.options.selectedBackColor && (o = this.options.selectedBackColor)), this.options.highlightSearchResults && e.searchResult && !e.state.disabled && (this.options.searchResultColor && (t = this.options.searchResultColor), this.options.searchResultBackColor && (o = this.options.searchResultBackColor)), "color:" + t + ";background-color:" + o + ";"
    }, n.prototype.injectStyle = function () {
        this.options.injectStyle && !o.getElementById(this.styleId) && e('<style type="text/css" id="' + this.styleId + '"> ' + this.buildStyle() + " </style>").appendTo("head")
    }, n.prototype.buildStyle = function () {
        let e = ".node-" + this.elementId + "{";
        return this.options.color && (e += "color:" + this.options.color + ";"), this.options.backColor && (e += "background-color:" + this.options.backColor + ";"), this.options.showBorder ? this.options.borderColor && (e += "border:1px solid " + this.options.borderColor + ";") : e += "border:none;", e += "}", this.options.onhoverColor && (e += ".node-" + this.elementId + ":not(.node-disabled):hover{background-color:" + this.options.onhoverColor + ";}"), this.css + e
    }, n.prototype.template = {
        list: '<ul class="list-group"></ul>',
        itemWrapper: '<li class="list-group-item d-flex justify-content-between align-items-start"></li>',
        itemLeftElem: '<div class="w-100"></div>',
        itemRightElem: "<div></div>",
        indent: '<span class="mx-2"></span>',
        icon: '<span class="icon"></span>',
        link: '<a class="w-75" href="#" style="display:inline-block; color:inherit; text-decoration:none;"></a>',
        badge: '<span class="ms-1 badge bg-primary rounded-pill"></span>'
    }, n.prototype.css = ".treeview .list-group-item{cursor:pointer}.treeview span.icon{width:12px;margin-right:5px}.treeview .node-disabled{color:silver;cursor:not-allowed}", n.prototype.getNode = function (e) {
        return this.nodes[e]
    }, n.prototype.getParent = function (e) {
        let t = this.identifyNode(e);
        return this.nodes[t.parentId]
    }, n.prototype.getSiblings = function (e) {
        let t = this.identifyNode(e), o = this.getParent(t);
        return (o ? o.nodes : this.tree).filter(function (e) {
            return e.nodeId !== t.nodeId
        })
    }, n.prototype.getSelected = function () {
        return this.findNodes("true", "g", "state.selected")
    }, n.prototype.getUnselected = function () {
        return this.findNodes("false", "g", "state.selected")
    }, n.prototype.getExpanded = function () {
        return this.findNodes("true", "g", "state.expanded")
    }, n.prototype.getCollapsed = function () {
        return this.findNodes("false", "g", "state.expanded")
    }, n.prototype.getChecked = function () {
        return this.findNodes("true", "g", "state.checked")
    }, n.prototype.getUnchecked = function () {
        return this.findNodes("false", "g", "state.checked")
    }, n.prototype.getDisabled = function () {
        return this.findNodes("true", "g", "state.disabled")
    }, n.prototype.getEnabled = function () {
        return this.findNodes("false", "g", "state.disabled")
    }, n.prototype.selectNode = function (t, o) {
        this.forEachIdentifier(t, o, e.proxy(function (e, t) {
            this.setSelectedState(e, !0, t)
        }, this)), this.render()
    }, n.prototype.unselectNode = function (t, o) {
        this.forEachIdentifier(t, o, e.proxy(function (e, t) {
            this.setSelectedState(e, !1, t)
        }, this)), this.render()
    }, n.prototype.toggleNodeSelected = function (t, o) {
        this.forEachIdentifier(t, o, e.proxy(function (e, t) {
            this.toggleSelectedState(e, t)
        }, this)), this.render()
    }, n.prototype.collapseAll = function (t) {
        let o = this.findNodes("true", "g", "state.expanded");
        this.forEachIdentifier(o, t, e.proxy(function (e, t) {
            this.setExpandedState(e, !1, t)
        }, this)), this.render()
    }, n.prototype.collapseNode = function (t, o) {
        this.forEachIdentifier(t, o, e.proxy(function (e, t) {
            this.setExpandedState(e, !1, t)
        }, this)), this.render()
    }, n.prototype.expandAll = function (t) {
        if ((t = e.extend({}, i.options, t)) && t.levels) this.expandLevels(this.tree, t.levels, t); else {
            let o = this.findNodes("false", "g", "state.expanded");
            this.forEachIdentifier(o, t, e.proxy(function (e, t) {
                this.setExpandedState(e, !0, t)
            }, this))
        }
        this.render()
    }, n.prototype.expandNode = function (t, o) {
        this.forEachIdentifier(t, o, e.proxy(function (e, t) {
            this.setExpandedState(e, !0, t), e.nodes && t && t.levels && this.expandLevels(e.nodes, t.levels - 1, t)
        }, this)), this.render()
    }, n.prototype.expandLevels = function (t, o, s) {
        s = e.extend({}, i.options, s), e.each(t, e.proxy(function (e, t) {
            this.setExpandedState(t, o > 0, s), t.nodes && this.expandLevels(t.nodes, o - 1, s)
        }, this))
    }, n.prototype.revealNode = function (t, o) {
        this.forEachIdentifier(t, o, e.proxy(function (e, t) {
            let o = this.getParent(e);
            for (; o;) this.setExpandedState(o, !0, t), o = this.getParent(o)
        }, this)), this.render()
    }, n.prototype.toggleNodeExpanded = function (t, o) {
        this.forEachIdentifier(t, o, e.proxy(function (e, t) {
            this.toggleExpandedState(e, t)
        }, this)), this.render()
    }, n.prototype.checkAll = function (t) {
        let o = this.findNodes("false", "g", "state.checked");
        this.forEachIdentifier(o, t, e.proxy(function (e, t) {
            this.setCheckedState(e, !0, t)
        }, this)), this.render()
    }, n.prototype.checkNode = function (t, o) {
        this.forEachIdentifier(t, o, e.proxy(function (e, t) {
            this.setCheckedState(e, !0, t)
        }, this)), this.render()
    }, n.prototype.uncheckAll = function (t) {
        let o = this.findNodes("true", "g", "state.checked");
        this.forEachIdentifier(o, t, e.proxy(function (e, t) {
            this.setCheckedState(e, !1, t)
        }, this)), this.render()
    }, n.prototype.uncheckNode = function (t, o) {
        this.forEachIdentifier(t, o, e.proxy(function (e, t) {
            this.setCheckedState(e, !1, t)
        }, this)), this.render()
    }, n.prototype.toggleNodeChecked = function (t, o) {
        this.forEachIdentifier(t, o, e.proxy(function (e, t) {
            this.toggleCheckedState(e, t)
        }, this)), this.render()
    }, n.prototype.disableAll = function (t) {
        let o = this.findNodes("false", "g", "state.disabled");
        this.forEachIdentifier(o, t, e.proxy(function (e, t) {
            this.setDisabledState(e, !0, t)
        }, this)), this.render()
    }, n.prototype.disableNode = function (t, o) {
        this.forEachIdentifier(t, o, e.proxy(function (e, t) {
            this.setDisabledState(e, !0, t)
        }, this)), this.render()
    }, n.prototype.enableAll = function (t) {
        let o = this.findNodes("true", "g", "state.disabled");
        this.forEachIdentifier(o, t, e.proxy(function (e, t) {
            this.setDisabledState(e, !1, t)
        }, this)), this.render()
    }, n.prototype.enableNode = function (t, o) {
        this.forEachIdentifier(t, o, e.proxy(function (e, t) {
            this.setDisabledState(e, !1, t)
        }, this)), this.render()
    }, n.prototype.toggleNodeDisabled = function (t, o) {
        this.forEachIdentifier(t, o, e.proxy(function (e, t) {
            this.setDisabledState(e, !e.state.disabled, t)
        }, this)), this.render()
    }, n.prototype.forEachIdentifier = function (t, o, s) {
        o = e.extend({}, i.options, o), t instanceof Array || (t = [t]), e.each(t, e.proxy(function (e, t) {
            s(this.identifyNode(t), o)
        }, this))
    }, n.prototype.identifyNode = function (e) {
        return "number" == typeof e ? this.nodes[e] : e
    }, n.prototype.search = function (t, o) {
        o = e.extend({}, i.searchOptions, o), this.clearSearch({render: !1});
        let s = [];
        if (t && t.length > 0) {
            o.exactMatch && (t = "^" + t + "$");
            let i = "g";
            o.ignoreCase && (i += "i"), s = this.findNodes(t, i), e.each(s, function (e, t) {
                t.searchResult = !0
            })
        }
        return o.revealResults ? this.revealNode(s) : this.render(), this.$element.trigger("searchComplete", e.extend(!0, {}, s)), s
    }, n.prototype.clearSearch = function (t) {
        t = e.extend({}, {render: !0}, t);
        let o = e.each(this.findNodes("true", "g", "searchResult"), function (e, t) {
            t.searchResult = !1
        });
        t.render && this.render(), this.$element.trigger("searchCleared", e.extend(!0, {}, o))
    }, n.prototype.findNodes = function (t, o, s) {
        o = o || "g", s = s || "text";
        let i = this;
        return e.grep(this.nodes, function (e) {
            let n = i.getNodeValue(e, s);
            if ("string" == typeof n) return n.match(new RegExp(t, o))
        })
    }, n.prototype.getNodeValue = function (e, t) {
        let o = t.indexOf(".");
        if (o > 0) {
            let s = e[t.substring(0, o)], i = t.substring(o + 1, t.length);
            return this.getNodeValue(s, i)
        }
        return e.hasOwnProperty(t) ? e[t].toString() : void 0
    };
    let d = function (e) {
        t.console && t.console.error(e)
    };
    e.fn.treeview = function (t, o) {
        let s;
        return this.each(function () {
            let i = e.data(this, "treeview");
            "string" == typeof t ? i ? e.isFunction(i[t]) && "_" !== t.charAt(0) ? (o instanceof Array || (o = [o]), s = i[t].apply(i, o)) : d("No such method : " + t) : d("Not initialized, can not call method : " + t) : "boolean" == typeof t ? s = i : e.data(this, "treeview", new n(this, e.extend(!0, {}, t)))
        }), s || this
    }
}(jQuery, window, document);