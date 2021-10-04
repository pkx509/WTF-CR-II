Ext.selection.TreeModel.prototype.onKeyUp = Ext.Function.createInterceptor(Ext.selection.TreeModel.prototype.onKeyUp, function () {
    var store = this.store,
        idx  = store.indexOf(this.lastFocused),
        visIdx,
        record,
        focusSearchField;

    if (idx === 0) {
        focusSearchField = true;
    } else {
        record = store.getAt(idx - 1);
        visIdx = idx - 1;

        while (visIdx > 0 && record.data.hidden) {
            record = store.getAt(--visIdx);
        }

        if (record.data.hidden) {
            focusSearchField = true;
        }
    }

    if (focusSearchField) {
        this.deselectAll();
        App.TriggerField1.focus(false, 100);
    }
});

var SEARCH_URL = "/search/",
    TAG_CLOUD_TOKEN = "/TagCloud",
    lockHistoryChange = false;

var tagLabelConfig = {
    trackOver: true,
    listeners: {
        beforetagadd: function (field, tag, o) {
            tag.overCls = "example-tag";
            o["data-qtip"] = "Click to filter examples by '" + tag.text + "'";
        },
        click: function (field, tag) {
            var searchField = App.TriggerField1;

            searchField.setValue(tag.text);
            changeFilterHash(tag.text.replace(" ", "+"));
            filter(searchField, true);
        }
    }
};

var makeTab = function (id, url, title,icon) {
    var win, 
        tab, 
        hostName, 
        exampleName, 
        //node = App.exampleTree.getStore().getNodeById(id), 
        tabTip;
    
    if (id === "-") {
        id = Ext.id(undefined, "extnet");
        lookup[url] = id;
    }
    
    tabTip = url.replace(/^\//g, "");
    tabTip = tabTip.replace(/\/$/g, "");
    tabTip = tabTip.replace(/\//g, " > ");
    tabTip = tabTip.replace(/_/g, " ");
    
    win = new Ext.Window({
        id      : "w" + id,
        layout  : "fit",        
        title   : title,
        iconCls : icon,
        width   : 925,
        height  : 650,
        border  : false,
        maximizable : true,
        constrain   : true,
        closeAction : "hide",        
        listeners   : {
            show : {
                fn : function () {
                    var height = Ext.getBody().getViewSize().height;
                    
                    if (this.getSize().height > height) {
                        this.setHeight(height - 20)
                    }

                    this.body.mask("Loading...", "x-mask-loading");
                    //Ext.Ajax.request({
                    //    url     : "ExampleLoader.ashx",
                    //    success : function (response) { 
                    //        this.body.unmask();
                    //        eval(response.responseText);
                    //    },
                    //    failure : function (response) {
                    //        this.body.unmask();
                    //        Ext.Msg.alert("Failure", "The error during example loading:\n" + response.responseText);
                    //    },
                    //    params : { 
                    //        id  : id, 
                    //        url : url, 
                    //        wId : this.nsId
                    //    },
                    //    scope : this
                    //});
                },
                
                single : true
            }
        },
        buttons :[
            {
                id   : "b" + id,
                text : "Download",
                iconCls   : "#Compress",
                listeners : {
                    click : {
                        fn : function (el, e) {
                            window.location = "/GenerateSource.ashx?t=1&e=" + url;
                        }
                    }
                }
            }
        ]        
    });
    
    hostName = window.location.protocol + "//" + window.location.host;
    exampleName = url;
    
    tab = App.ExampleTabs.add(new Ext.Panel({
        id   : id,  
        title    : title,
        tabTip   : tabTip,
        hideMode : "offsets",        
        iconCls  : icon,
        loader : {
            scripts   : true,
            url       : hostName  + url,
            renderer  : "frame",
            listeners : {
                beforeload: function() {
                    this.target.body.mask('Loading...');
                },
                load: {
                    fn: function (loader) {
                        var frame = loader.target.getBody();

                        if (!frame.Ext) {
                            swapThemeClass(frame, "", Ext.net.ResourceMgr.theme);
                        }

                        this.target.body.unmask();
                    }
                }
            }
        },
        listeners : {
            deactivate : {
                fn : function (el) {
                    if (this.sWin && this.sWin.isVisible()) {
                         this.sWin.hide();
                    }
                }
            },
            
            destroy : function () {
                if (this.sWin) {
                    this.sWin.close();
                    this.sWin.destroy();
                }
            }
        },
        closable : true
    }));
    
    tab.sWin = win;
    setTimeout(function () {
        App.ExampleTabs.setActiveTab(tab);
    }, 250);
    
    var expandAndSelect = function (node) {
            var view = App.exampleTree.getView(),
                originalAnimate = view.animate;

            view.animate = false;
            node.bubble(function (node) {
                node.expand(false);
            }); 

            App.exampleTree.getSelectionModel().select(node);
            view.animate = originalAnimate;
        };
         
    //if (node) {
    //    expandAndSelect(node);  
    //    //createTagItems(tab, node);
    //} else {
    //    App.exampleTree.on("load", function (node) {
    //        node = App.exampleTree.getStore().getNodeById(id);
    //        if (node) {
    //            expandAndSelect(node);
    //            //createTagItems(tab, node);
    //        }
    //    }, this, { delay: 10, single : true });
    //}
};

var lookup = {};

var onTreeAfterRender = function (tree) {
    var sm = tree.getSelectionModel();

    Ext.create('Ext.util.KeyNav', tree.view.el, {
        enter : function (e) {
            if (sm.hasSelection()) {
                onTreeItemClick(sm.getSelection()[0], e);
            }
        }
    });
};

var onTreeItemClick = function (record, e) {
    if (record.isLeaf()) { 
        e.stopEvent(); 
        loadExample(record.get('hrefTarget'), record.data.id, record.get('text'), record.get('iconCls'));
    } else {
        record[record.isExpanded() ? 'collapse' : 'expand']();
    }
};

var treeRenderer = function (value, metadata, record) {
    if (record.data.isNew) {
        value += "<span>&nbsp;</span>";
    }
    
    return value;
};

var loadExample = function (href, id, title,icon) {
    var tab = App.ExampleTabs.getComponent(id),
        lObj = lookup[href];
        
    if (id == "-") {
        App.direct.GetHashCode(href,{
            success : function (result) {
                loadExample(href, "e" + result, title, icon);
            }
        });
        
        return;
    }
    
    lookup[href] = id;

    if (tab) {
        App.ExampleTabs.setActiveTab(tab);
    } else {
        if (Ext.isEmpty(title)) {
            var m = /(\w+)\/$/g.exec(href);
            title = m == null ? "[No Name]" : m[1];
        }
        
        title = title.replace(/_/g, " ");
        makeTab(id, href, title,icon);
    }
};

var viewClick = function (dv, e) {
    var group = e.getTarget("h2", 3, true);

    if (group) {
        group.up("div").toggleClass("collapsed");
    }
};

var beforeSourceShow = function (el) {
    var height = Ext.getBody().getViewSize().height;
    
    if (el.getSize().height > height) {
        el.setHeight(height - 20);
    }
};

var change = function (token) {
    //if (!lockHistoryChange) {
    //    if (token) {
    //        if (token.indexOf(SEARCH_URL) === 0) {
    //            filterByUrl(token);
    //        } else if (token === TAG_CLOUD_TOKEN) {
    //            showTagCloud();
    //        } else {
    //            loadExample(token, lookup[token] || "-" );
    //        }
    //    } else {
    //        App.ExampleTabs.setActiveTab(0);
    //    }
    //}

    //lockHistoryChange = false;
};

var getToken = function (url) {
    var host = window.location.protocol + "//" + window.location.host;
    var subUrl = url.substr(host.length - 1);
    return subUrl.substr(0, subUrl.length - 5);
};

var addToken = function (el, tab) {
    if (tab.loader && tab.loader.url) {
        var token = tab.loader.target.title;//getToken(tab.loader.url);
        
        Ext.History.add(token);
    } else if (tab.historyToken) {
        Ext.History.add(tab.historyToken);
    } else {
        Ext.History.add("");
    }
};

var keyUp = function (field, e) {
    if (e.getKey() === 40) {
        return;
    }

    if (e.getKey() === Ext.EventObject.ESC) {
        clearFilter(field);
    } else {
        changeFilterHash(field.getRawValue().replace(" ", "+"));
        filter(field);
    }
};

/*
    field: the search field
    byTagsOnly: true means searcing by tags only and by full matching
*/
var filter = function (field, byTagsOnly) {
    var tree = App.exampleTree,
        text = field.getRawValue(),
        view = tree.getView(),
        originalAnimate = view.animate;
    
    if (Ext.isEmpty(text, false)) {
        clearFilter(field);
    }
    
    if (text.length < 2) {
        return;
    }
    
    if (Ext.isEmpty(text, false)) {
        return;
    }
    
    field.getTrigger(0).show();
    
    var re = new RegExp(".*" + text + ".*", "i");
        
    tree.clearFilter(true);

    tree.filterBy(function (node) {
        var tags = node.data.tags,
            hasTags = Ext.isArray(node.data.tags) && node.data.tags.length > 0,
            match = false,
            pn = node.parentNode,
            i, len, 
            tagMatch = false;

        if (App.SearchByTitles.checked && !byTagsOnly) {
            match = re.test(node.data.text);
        }

        if ((App.SearchByTags.checked || byTagsOnly) && hasTags) {
            if (byTagsOnly) {
                match = match || Ext.Array.contains(tags, text);
            } else {
                for (i = 0, len = tags.length; i < len; i++) {
                    if (re.test(tags[i])) {
                        match = true;
                        break;
                    }
                }
            }
        }

        if (match && node.isLeaf()) {
            pn.hasMatchNode = true;
        }

        if (pn != null && pn.fixed) {
            if (node.isLeaf() === false) {
                node.fixed = true;
            }
            return true;
        }

        if (node.isLeaf() === false) {
            node.fixed = match;
            return match;
        }

        return (pn != null && pn.fixed) || match;
    }, { expandNodes : false });

    view.animate = false;
    tree.getRootNode().cascadeBy(function (node) {
        if (node.isRoot()) {
            return;
        }

        if ((node.getDepth() === 1) || 
            (node.getDepth() === 2 && node.hasMatchNode)) {
            node.expand(false);
        }

        delete node.fixed;
        delete node.hasMatchNode;
    }, tree);

    view.animate = originalAnimate;
};

var filterByUrl = function (url) {
    var field = App.TriggerField1,
        tree = App.exampleTree;

    if (!lockHistoryChange) {
        var tree = App.exampleTree,
            store = tree.getStore(),
            fn = function () {
                field.setValue(url.substr(SEARCH_URL.length).replace("+", " "));
                filter(field);
            };

        if (store.loading) {
            store.on("load", fn, null, { single : true, delay: 100 });
        } else {
            fn();
        }
    }
};

var clearFilter = function (field, trigger, index, e) {
    var tree = App.exampleTree;
    
    field.setValue("");
    changeFilterHash("");
    field.getTrigger(0).hide();
    tree.clearFilter(true);
    field.focus(false, 100);
};

var changeFilterHash = Ext.Function.createBuffered(
    function (text) {
        lockHistoryChange = true;

        if (text.length > 2) {
            window.location.hash = SEARCH_URL + text;
        } else {
            var tab = App.ExampleTabs.getActiveTab(),
                token = "";

            if (tab.loader && tab.loader.url) {
                token = getToken(tab.loader.url);
            }
        
            Ext.History.add(token);
        }
    },
    500);

var filterSpecialKey = function (field, e) {
    var tree = App.exampleTree,
        view = tree.getView();

    if (e.getKey() === e.DOWN) {
        var n = tree.getRootNode().findChildBy(function (node) {
            return node.isLeaf() && !node.data.hidden;
        }, tree, true);
        
        if (n) {
            tree.expandPath(n.getPath(), null, null, function () {
                tree.getSelectionModel().select(n);
                view.focusRow(n);
            });
        }
    }
};

var filterNewExamples = function (checkItem, checked) {
    var tree = App.exampleTree;
        
    if (checked) {
        tree.clearFilter(true);
        tree.filterBy(function (node) {
            return node.data.isNew;
        });    
    } else {
        tree.clearFilter(true);
    }
};

var swapThemeClass = function (frame, oldTheme, newTheme) {
    var html = Ext.fly(frame.document.body.parentNode);
                                                                        
    html.removeCls('x-theme-' + oldTheme);
    html.addCls('x-theme-' + newTheme);
};

var themeChange = function (menu, menuItem) {
    var reload = menuItem.text == "Neptune" || Ext.net.ResourceMgr.theme == "neptune";

    App.direct.GetThemeUrl(menuItem.text, reload, {
		success : function (result) {
            var v = menu.up('viewport'),
                oldTheme = Ext.net.ResourceMgr.theme,
                html,
                frame;

            Ext.net.ResourceMgr.setTheme(result, menuItem.text.toLowerCase());
            Ext.defer(v.doLayout, 500, v);
			App.ExampleTabs.items.each(function (el) {
				if (!Ext.isEmpty(el.iframe)) {
                    frame = el.getBody();
					if (frame.Ext) {
						frame.Ext.net.ResourceMgr.setTheme(result, menuItem.text.toLowerCase());
					} else {
                        swapThemeClass(frame, oldTheme, Ext.net.ResourceMgr.theme);
                    }
				}
			});
		}
	});
};

var loadComments = function (at, url) {
    App.winComments.url = url;
    
    App.winComments.show(at, function () {
        updateComments(false, url);
        App.TagsView.store.reload();
    });
};

var updateComments = function (updateCount, url) {
    winComments.body.mask("Loading...", "x-mask-loading");
    Ext.net.DirectMethod.request({
        url: "/ExampleLoader.ashx",
        cleanRequest : true,
        params       : {
            url : url,
            action : "comments.build"                            
        },
        success      : function (result, response, extraParams, o) {
            if (result && result.length > 0) {
                App.tplComments.overwrite(CommentsBody.body, result);
            }
            
            if (updateCount) {
                App.ExampleTabs.getActiveTab().commentsBtn.setText("Comments ("+result.length+")");
            }
        },
        complete    : function (success, result, response, extraParams, o) {
            App.winComments.body.unmask();
        }
    });
};

var getAllTags = function () {
    var tags = [],
        root = App.exampleTree.getRootNode();
        
    root.cascadeBy(function (node) {
        if (Ext.isArray(node.data.tags)) {    
            tags = tags.concat(node.data.tags)
        }
    });

    tags = Ext.Array.unique(tags);

    return Ext.Array.sort(tags);
};

var showTagCloud = function () {
    var tabPanel = App.ExampleTabs,
        tabId = "tagCloudTab",
        tab = tabPanel.getComponent(tabId);

    if (!tab) {
        tab = Ext.create("Ext.panel.Panel", {
            id: tabId,
            title: "Tag Cloud",
            icon: "#WeatherClouds",
            closable: true,
            url: "TagCloud",
            margin: 20,
            historyToken: TAG_CLOUD_TOKEN,
            items: [
                Ext.applyIf({
                    xtype: "taglabel",            
                    tags: getAllTags()
                }, tagLabelConfig)
            ]
        });

        tabPanel.addTab(tab);
    }

    tabPanel.setActiveTab(tab);
};

if (window.location.href.indexOf("#") > 0) {
    var directLink = window.location.href.substr(window.location.href.indexOf("#"))
                        .replace(/(\s|\<|&lt;|%22|%3C|\"|\'|&quot|&#039;|script)/gi, '');
    
    Ext.onReady(function () {
        Ext.Function.defer(function () {
            if (directLink.indexOf(SEARCH_URL) === 0) {
                filterByUrl(directLink);
            } else {
                if (!Ext.isEmpty(directLink, false)) {
                    if (directLink === TAG_CLOUD_TOKEN) {
                        if (App.exampleTree.store.loading) {
                            App.exampleTree.store.on("load", function () {
                                showTagCloud();
                            }, null, { single: true })
                        } else {
                            showTagCloud();
                        }
                    } else {
                        //loadExample(directLink, "-");
                    }
                }
            }
        }, 100, window);
    }, window);
}