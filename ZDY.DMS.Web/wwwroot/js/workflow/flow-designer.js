var designer = function () {
    var color = zrender.color;
    var util = zrender.util;
    var Group = zrender.Group;
    var Circle = zrender.Circle;
    var Rectangle = zrender.Rect;
    var Polygon = zrender.Polygon;
    var Polyline = zrender.Polyline;

    function Guid(g) {

        var arr = new Array(); //存放32位数值的数组
        if (typeof g === "string") { //如果构造函数的参数为字符串
            InitByString(arr, g);
        }
        else {
            InitByOther(arr);
        }
        //返回一个值，该值指示 Guid 的两个实例是否表示同一个值。
        this.Equals = function (o) {
            if (o && o.IsGuid) {
                return this.ToString() === o.ToString();
            }
            else {
                return false;
            }
        };

        //Guid对象的标记
        this.IsGuid = function () { };
        //返回 Guid 类的此实例值的 String 表示形式。
        this.ToString = function (format) {
            if (typeof format === "string") {
                if (format === "N" || format === "D" || format === "B" || format === "P") {
                    return ToStringWithFormat(arr, format);
                }
                else {
                    return ToStringWithFormat(arr, "D");
                }
            }
            else {
                return ToStringWithFormat(arr, "D");
            }
        };

        //由字符串加载
        function InitByString(arr, g) {
            g = g.replace(/\{|\(|\)|\}|-/g, "");
            g = g.toLowerCase();
            if (g.length !== 32 || g.search(/[^0-9,a-f]/i) !== -1) {
                InitByOther(arr);
            }
            else {
                for (var i = 0; i < g.length; i++) {
                    arr.push(g[i]);
                }
            }
        }
        //由其他类型加载
        function InitByOther(arr) {
            var i = 32;
            while (i--) {
                arr.push("0");
            }
        }

        /*
        根据所提供的格式说明符，返回此 Guid 实例值的 String 表示形式。
        N  32 位： xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        D  由连字符分隔的 32 位数字 xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx 
        B  括在大括号中、由连字符分隔的 32 位数字：{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx} 
        P  括在圆括号中、由连字符分隔的 32 位数字：(xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx) 
        */

        function ToStringWithFormat(arr, format) {
            var str;

            switch (format) {
                case "N":
                    return arr.toString().replace(/,/g, "");
                case "D":
                    str = arr.slice(0, 8) + "-" + arr.slice(8, 12) + "-" + arr.slice(12, 16) + "-" + arr.slice(16, 20) + "-" + arr.slice(20, 32);
                    str = str.replace(/,/g, "");
                    return str;
                case "B":
                    str = ToStringWithFormat(arr, "D");
                    str = "{" + str + "}";
                    return str;
                case "P":
                    str = ToStringWithFormat(arr, "D");
                    str = "(" + str + ")";
                    return str;
                default:
                    return new Guid();
            }
        }
    }

    function NewGuid() {
        var g = "";
        var i = 32;
        while (i--) {
            g += Math.floor(Math.random() * 16.0).toString(16);
        }
        return new Guid(g);
    }

    function Graph() {
        var canvasElement = document.getElementById('canvas'),
            graph = this,
            activities = {},
            transitions = {};
        var zrenderInstance,
            contextMenuContainer;

        this.zrenderInstance = function () {
            return zrenderInstance;
        };
        this.newGuid = function () {
            var guid = NewGuid().ToString();
            if (activities[guid] !== undefined || transitions[guid] !== undefined) {
                guid = graph.newGuid();
            }
            return guid;
        };

        this.addActivity = function (activity) {
            activities[activity.id] = activity;
        };
        this.removeActivity = function (activityId) {
            delete activities[activityId];
        };
        this.getActivity = function (id) { return activities[id] === undefined ? undefined : activities[id]; };
        this.getActivities = function () {
            var array = [];
            for (var activityId in activities) {
                array.push(this.getActivity(activityId));
            }
            return array;
        };
        this.clearActivity = function () {
            activities = {};
        };

        this.addTransition = function (transition) {
            transitions[transition.id] = transition;
        };
        this.removeTransition = function (transitionId) {
            delete transitions[transitionId];
        };
        this.getTransition = function (id) { return transitions[id] === undefined ? undefined : transitions[id]; };
        this.getTransitions = function () {
            var array = [];
            for (var transitionId in transitions) {
                array.push(this.getTransition(transitionId));
            }
            return array;
        };
        this.clearTransition = function () {
            transitions = {};
        };

        this.connectActivitys = {
            activities: [],
            subtype: 1,
            isWaiting: function () {
                return this.activities.length > 0;
            },
            setType: function (type) {
                this.subtype = type;
                return this;
            },
            clear: function () {
                this.subtype = 1;
                this.activities = [];
                return this;
            },
            add: function (activity) {
                this.activities.push(activity);
                return this;
            },
            done: function () {
                if (this.activities.length === 2) {
                    var item = {
                        id: NewGuid().ToString(),
                        from: this.activities[0].id,
                        fromAngle: -5,
                        to: this.activities[1].id,
                        toAngle: -1,
                        title: '无条件',
                        subtype: this.subtype
                    };
                    var transition = TransitionSwitch(item);
                    transition.drawTo(graph);
                }
                this.activities = [];
            }
        };

        //创建布局图菜单
        ContextMenu.apply(this);
        this.contextMenuItems = [];
        this.contextMenuItems.push(new ContextMenuExecuteItem({ text: '新建步骤', action: 'add', source: { graph: this } }));
        this.contextMenuItems.push(new ContextMenuExecuteItem({ text: '新建子流程', action: 'add2', source: { graph: this } }));
        this.contextMenuItems.push(new ContextMenuExecuteItem({ text: '清空画布', action: 'clear', source: { graph: this } }));

        // 当前正在拖放的节点
        var dragingActivity = null;
        // 活动节点拖放开始
        this.onActivityDragStart = function (params, activity) { dragingActivity = activity; };
        // 活动节点拖放结束
        this.onActivityDragEnd = function (params, activity) {
            if (dragingActivity) {
                refreshActivityTransitions(dragingActivity);
            }
            dragingActivity = null;
        };
        // 刷新活动相关的所有连接弧
        function refreshActivityTransitions(activity) {
            var trans = graph.getActivityTransitions(activity);
            for (var i = 0; i < trans.length; i++) {
                trans[i].refresh();
            }
        }
        // 获取所有相关的连接弧
        this.getActivityTransitions = function (activity) {
            var activityId = activity.id;
            var trans = [];
            for (var key in transitions) {
                var transition = transitions[key];
                if (transition.from === activityId || transition.to === activityId) {
                    trans.push(transition);
                }
            }
            return trans;
        };
        // 拖动过程处理
        function zrenderInstanceOnMouseMove(params) {
            var event = params.event;
            if (dragingActivity) {
                refreshActivityTransitions(dragingActivity);
            }
            if (dragingTransition) {
                dragingTransition.updateMiddlePoint(dragingMiddlePointIndex, event.zrX, event.zrY).clearMovement().refresh();
            }
        }

        // 当前正在拖放的连接弧，拖动连接弧，会增加中间节点
        var dragingTransition = null;
        var dragingMiddlePointIndex = -1;
        var dragingTransitionStartPoint;
        this.onTransitionDragStart = function (transition, event) {
            dragingTransition = transition;
            dragingTransitionStartPoint = { x: event.clientX, y: event.clientY };
            dragingMiddlePointIndex = transition.addMiddlePoint(event.clientX, event.clientY);
        };
        this.onTransitionDragEnd = function (event) {
            if (!dragingTransition) return;
            dragingTransition.refresh();
            // 计算拖动距离
            var dragDistance = Math.sqrt((event.clientX - dragingTransitionStartPoint.x) * (event.clientX - dragingTransitionStartPoint.x) + (event.clientY - dragingTransitionStartPoint.y) * (event.clientY - dragingTransitionStartPoint.y));
            if (dragDistance > 8) {
                var middlePoint = dragingTransition.middlePoints[dragingMiddlePointIndex];
            } else {
                // 恢复原状
                dragingTransition.deleteMiddlePoint(dragingMiddlePointIndex).refresh();
            }
            dragingTransition = null;
            dragingMiddlePointIndex = -1;
        };
        this.onTransitionMiddlePointDragStart = function (transition, index) {
            dragingTransition = transition;
            dragingMiddlePointIndex = index;
        };
        this.onTransitionMiddlePointDragEnd = function () {
            if (!dragingTransition) return;
            dragingTransition.refresh();
            // 修改节点到服务器端
            var middlePoint = dragingTransition.middlePoints[dragingMiddlePointIndex];

            dragingTransition = null;
            dragingMiddlePointIndex = -1;
        };

        // 当前选中的部件
        var selectedUnit = null;
        this.onUnitSelect = function (params, unit) {
            if (selectedUnit) selectedUnit.unselect();
            unit.select();
            selectedUnit = unit;
        };

        // 记录当前鼠标在哪个部件上，可以用来生成上下文相关菜单
        var currentUnit = null;
        this.onUnitMouseOver = function (params, unit) {
            currentUnit = unit;
        };
        this.onUnitMouseOut = function (params, unit) {
            if (currentUnit === unit) currentUnit = null;
        };
        // 上下文菜单事件响应
        this.onContextMenu = function (event) {
            if (currentUnit)
                currentUnit.showContextMenu(event, contextMenuContainer, graph);
            else
                graph.showContextMenu(event, contextMenuContainer, graph);
            return false;
        };

        // 放大缩小
        this.handleWheel = function (isEnlarged) {
            if (isEnlarged) {
                this.scale += 0.2;
            } else {
                this.scale -= 0.2;
            }

            if (this.scale < 0.5 || this.scale > 3) {
                if (this.scale < 0.5) {
                    this.scale = 0.5;
                } else if (this.scale > 3) {
                    this.scale = 3;
                }
                return;
            }
            var storage = this.zrenderInstance().storage;
            var els = this.zrenderInstance().storage.getDisplayList(true, true);
            for (var i = 0; i < els.length; i++) {
                var el = els[i];
                if (el.tag !== "tool") {
                    el.attr("scale", [this.scale, this.scale]);
                }
            }
            this.zrenderInstance().refresh();
        };
        this.useWheel = function () {
            if (!graph.designable) {
                var wheelTool = new WheelTool();
                wheelTool.addTo(graph);
            }
        };

        // 整体拖动
        var moveState,
            lastMouseDownPosition;
        this.useMove = function () {
            if (!graph.designable) {
                zrenderInstance.on('mousedown', function (e) {
                    lastMouseDownPosition = [e.event.zrX, e.event.zrY];
                    moveState = true;

                });

                zrenderInstance.on('mouseup', function (e) {
                    moveState = false;
                });

                zrenderInstance.on('mousemove', function (e) {
                    if (moveState) {
                        var xdiff = e.event.zrX - lastMouseDownPosition[0];
                        var ydiff = e.event.zrY - lastMouseDownPosition[1];

                        //修改每个元素的偏移量 position
                        var els = zrenderInstance.storage.getDisplayList(true, true);
                        for (var i = 0; i < els.length; i++) {
                            var el = els[i];
                            if (el.tag !== "tool") {
                                el.position[0] = el.position[0] + xdiff;
                                el.position[1] = el.position[1] + ydiff;
                                el.dirty();
                            }
                        }

                        zrenderInstance.refresh();

                        lastMouseDownPosition = [e.event.zrX, e.event.zrY];
                    }
                });
            }
        };

        // 初始化
        this.init = function (options) {
            if (options !== undefined) {
                this.offsetX = options.offsetX || canvasElement.offsetLeft;
                this.offsetY = options.offsetY || canvasElement.offsetTop;
                this.fontFamily = options.fontFamily || 'Microsoft YaHei';
                this.designable = options.designable || false;
                this.scale = options.scale || 1;
                this.activityDataModel = options.activityDataModel || {};
                this.transitionDataModel = options.transitionDataModel || {};
                this.onUnitClick = options.onUnitClick;
                this.onUnitDblclick = options.onUnitDblclick;
                this.onUnitSetting = options.onUnitSetting;
            }

            canvasElement.style.height = (options.height || 600) + 'px';
            zrenderInstance = zrender.init(canvasElement);

            if (this.designable) {
                // 创建上下文菜单容器
                contextMenuContainer = document.createElement('div');
                contextMenuContainer.className = 'context-menu';
                contextMenuContainer.style.display = 'none';
                canvasElement.appendChild(contextMenuContainer);
                contextMenuContainer.onmouseleave = function (event) {
                    contextMenuContainer.style.display = 'none';
                };
                contextMenuContainer.onclick = function (event) {
                    contextMenuContainer.style.display = 'none';
                };

                // 侦听拖动过程
                zrenderInstance.on('mousemove', zrenderInstanceOnMouseMove);
                // 上下文菜单
                canvasElement.oncontextmenu = graph.onContextMenu;
            } else {
                canvasElement.oncontextmenu = function () { return false; };

                //使用整体拖动
                this.useMove();
            }
        };
        //新增元素
        this.add = function (element) {
            zrenderInstance.add(element);
        };
        //删除元素
        this.remove = function (element) {
            zrenderInstance.remove(element);
        };
        //清空元素
        this.clear = function () {
            zrenderInstance.clear();
            graph.clearActivity();
            graph.clearTransition();
            graph.connectActivitys.clear();

            //使用放大缩小工具
            graph.useWheel();
        };
        //刷新所有元素
        this.refresh = function () {
            for (var key in transitions) {
                var transition = transitions[key];
                transition.refresh();
            }
            for (var key2 in activities) {
                var activitiy = activities[key2];
                activitiy.refresh();
            }
            this.zrenderInstance().refresh();
        };
    }

    function Unit(options) {
        ContextMenu.apply(this);

        var unit = this;

        // 上下文菜单项集合
        this.contextMenuItems = [];
        // 属性
        this.id = options.id;
        this.title = options.title;
        this.data = options.data;
        this.clickable = options.clickable === undefined ? true : options.clickable;
        this.graph = null;
        this.element = null;
        // 当前是否被选中
        this.selected = false;
        this.createShapeOptions = function () {
            return {
                id: unit.id,
                scale: [unit.graph.scale, unit.graph.scale],
                onclick: function (params) {
                    // 选中并高亮
                    if (unit.graph.designable) unit.graph.onUnitSelect(params, unit);
                    // 连接节点
                    if (unit.type === 'Activity' && unit.graph.connectActivitys.isWaiting()) {
                        unit.graph.connectActivitys.add(unit).done();
                    }
                    if (typeof unit.graph.onUnitClick === 'function' && unit.clickable === true) {
                        unit.graph.onUnitClick(params, unit);
                    }
                },
                ondblclick: function (params) {
                    if (typeof unit.graph.onUnitDblclick === 'function' && unit.clickable === true) {
                        unit.graph.onUnitDblclick(params, unit);
                    }
                },
                onmouseover: function (params) {
                    if (unit.graph.designable) unit.graph.onUnitMouseOver(params, unit);
                },
                onmouseout: function (params) {
                    if (unit.graph.designable) unit.graph.onUnitMouseOut(params, unit);
                }
            };
        };
        this.addTo = function (graph) { };
        this.drawTo = function (graph) {
            this.graph = graph;
            if (this.type === 'Transition') {
                graph.addTransition(this);
            }
            else if (this.type === 'Activity') {
                graph.addActivity(this);
            }
            this.addTo(graph);
        };
        this.remove = function () { };
        // 刷新显示
        this.refresh = function (graph) { return []; };
        // 选中
        this.select = function () {
            this.selected = true;
            return this.refresh();
        };
        // 取消选中
        this.unselect = function () {
            this.selected = false;
            return this.refresh();
        };
    }

    function AngleTargets(activity, transition, fromOrTo) {
        var graph = activity.graph,
            elements = [],
            dropInvoked = false;

        function createShapeOptions(angle, position) {
            return {
                shape: {
                    cx: position.x,
                    cy: position.y,
                    r: 8
                },
                style: {
                    fill: '#fdc',
                    stroke: '#fff',
                    lineWidth: 2
                },

                ondrop: function (params) {
                    dropInvoked = true;
                    refreshAngle(angle);
                }
            };
        }

        // 修改连接符的信息，保存到服务器
        function refreshAngle(angle) {
            var fromAngle = transition.fromAngle,
                toAngle = transition.toAngle;
            if (fromOrTo) fromAngle = angle; else toAngle = angle;
            if (fromAngle === transition.fromAngle && toAngle === transition.toAngle) return;

            // 本地刷新
            transition.fromAngle = fromAngle;
            transition.toAngle = toAngle;
            transition.clearMovement().refresh();
        }

        this.start = function (graph) {
            for (var i = 1; i <= 8; i++) {
                var element = new Circle(createShapeOptions(i, activity.getAnglePosition(i)));
                graph.add(element);
                elements.push(element);
            }
        };

        this.end = function (graph) {
            if (!dropInvoked) {
                // 清除固定角
                refreshAngle(0);
            }
            dropInvoked = false;
            elements.each(function (element) { graph.remove(element); });
        };
    }

    function Activity(options) {
        Unit.apply(this, arguments);

        var activity = this;

        this.color = {
            // 活动图形填充色
            ACTIVITY_COLOR: 'rgba(215, 249, 255, 0.6)',
            // 活动线段颜色（如边框颜色）
            ACTIVITY_STROKE_COLOR: '#999',
            // 活动文字颜色
            ACTIVITY_TEXT_COLOR: '#333',
            // 活动（选中时）图形填充色
            ACTIVITY_SELECTED_COLOR: 'rgba(255, 102, 51, 0.6)',

            // 活动实例图形填充色
            ACTIVITY_INSTANCE_COLOR: 'rgba(221, 221, 221, 0.8)',
            // 活动实例图形填充色（已初始化）
            ACTIVITY_INSTANCE_INACTIVE_COLOR: 'rgba(39, 104, 234, 0.8)',
            // 活动实例图形填充色（等待中）
            ACTIVITY_INSTANCE_ACTIVE_COLOR: 'rgba(244, 208, 63, 0.8)',
            // 活动实例图形填充色（已挂起）
            ACTIVITY_INSTANCE_SUSPENDED_COLOR: 'rgba(231, 80, 90, 0.8)',
            // 活动实例图形填充色（已完成）
            ACTIVITY_INSTANCE_COMPLETED_COLOR: 'rgba(38, 194, 129, 0.8)',
            // 获取指定状态的活动实例图形填充色
            getActivityArchiveColor: function (status) {
                switch (status) {
                    case 0:
                        return activity.color.ACTIVITY_INSTANCE_INACTIVE_COLOR;
                    case 1:
                        return activity.color.ACTIVITY_INSTANCE_ACTIVE_COLOR;
                    case 2:
                        return activity.color.ACTIVITY_INSTANCE_SUSPENDED_COLOR;
                    case 3:
                        return activity.color.ACTIVITY_INSTANCE_COMPLETED_COLOR;
                    default:
                        return activity.color.ACTIVITY_INSTANCE_COLOR;
                }
            }
        };

        this.type = options.type || 'Activity';
        this.status = options.status || undefined;
        this.position = options.position || { x: 0, y: 0 };
        this.angleTargets = new AngleTargets(this);

        this.contextMenuItems = [];
        this.contextMenuItems.push(new ContextMenuExecuteItem({ text: '条件链接', action: 'join', source: this }));
        this.contextMenuItems.push(new ContextMenuExecuteItem({ text: '配置节点', action: 'setting', source: this }));
        this.contextMenuItems.push(new ContextMenuExecuteItem({ text: '删除节点', action: 'remove', source: this }));

        // 获取角点位置（从左上角开始0-7共8个点位）
        this.getAnglePosition = function (angleIndex) { };

        // 获取当前位置（因为可拖动，所以位置会变化）
        this.getCurrentPosition = function () {
            var position = this.element.position,
                shape = this.element.shape;
            return {
                x: shape.x + position[0],
                y: shape.y + position[1]
            };
        };

        // 创建图形配置项
        this.createActivityOptions = function () {
            var _this = this,
                graph = _this.graph,
                activityOptions = this.createShapeOptions();

            return util.merge(activityOptions, {
                shape: {
                    x: _this.position.x,
                    y: _this.position.y
                },
                style: {
                    fill: graph.designable ? _this.color.ACTIVITY_COLOR : _this.color.getActivityArchiveColor(_this.status),
                    stroke: _this.color.ACTIVITY_STROKE_COLOR,
                    lineWidth: 1,
                    fontFamily: graph.fontFamily,
                    text: _this.title,
                    textPosition: 'inside',
                    textFill: _this.color.ACTIVITY_TEXT_COLOR,
                    transformText: true
                },
                draggable: graph.designable,
                ondragstart: function (params) { graph.onActivityDragStart(params, _this); },
                ondragend: function (params) { graph.onActivityDragEnd(params, _this); }
            });
        };

        // 创建8个角分位点
        this.createAngleTargets = function () {
            var _this = this,
                graph = _this.graph,
                elements = [];
            for (var i = 1; i <= 8; i++) {
                var shapeOptions = _this.createShapeOptions();
                shapeOptions.shape.r = 5;
                shapeOptions.style.text = '';
                var xy = _this.getAnglePosition(i);
                shapeOptions.shape.cx = xy.x;
                shapeOptions.shape.cy = xy.y;
                var element = new Circle(shapeOptions);
                graph.add(element);
                elements.push(element);
            }
            return elements;
        };

        // 默认处理为中心点
        this.getMaxCoords = function () { return this.getCurrentPosition(); };

        // 删除图形
        this.remove = function () {
            this.graph.remove(this.element);
            this.graph.removeActivity(this.id);
            var trans = this.graph.getActivityTransitions(this);
            for (var i = 0; i < trans.length; i++) {
                trans[i].remove();
            }
        };
    }

    function BothEndsActivity(options) {
        Activity.apply(this, arguments);

        this.r = 30;
        this.subtype = 0;

        this.getAnglePosition = function (angleIndex) {
            var r = this.r,
                position = this.element.position,
                shape = this.element.shape,
                style = this.element.style;
            var radian = 2 * Math.PI / 360 * ((angleIndex + 2) % 8) * 45;
            return {
                x: Math.round(shape.cx + position[0] + Math.sin(radian) * r),
                y: Math.round(shape.cy + position[1] + Math.cos(radian) * r)
            };
        };
        this.createCircleOptions = function (selected) {
            var _this = this;
            var shapeOptions = _this.createActivityOptions();
            shapeOptions.shape.cx = shapeOptions.shape.x;
            shapeOptions.shape.cy = shapeOptions.shape.y;
            shapeOptions.shape.r = 30;
            if (selected) shapeOptions.style.fill = _this.color.ACTIVITY_SELECTED_COLOR;
            return shapeOptions;
        };
        this.getMaxCoords = function () {
            var currentPosition = this.getCurrentPosition();
            return { x: currentPosition.x + this.r, y: currentPosition.y + this.r };
        };
        this.addTo = function (graph) {
            var _this = this,
                activityDataModel = graph.activityDataModel,
                shapeOptions = _this.createCircleOptions(false);
            this.element = new Circle(shapeOptions);
            graph.add(this.element);

            var data = new activityDataModel(_this);
            this.data = this.data ? util.merge(this.data, data) : data;
        };
        this.refresh = function () {
            var _this = this, shapeOptions = _this.createCircleOptions(_this.selected);
            _this.element.setStyle(shapeOptions.style);
            return [_this.element];
        };
    }

    function SubFlowActivity(options) {
        Activity.apply(this, arguments);

        this.width = 110;
        this.height = 60;
        this.delta = 20;
        this.subtype = 1;
        this.buildPointList = function (width, height, delta) {
            var position = this.position, x = position.x, y = position.y, halfHeight = height / 2, halfWidthDelta = (width + delta) / 2;
            var topLeft = [x - halfWidthDelta + delta, y - halfHeight],
                topRight = [x + halfWidthDelta, y - halfHeight],
                bottomRight = [x + halfWidthDelta - delta, y + halfHeight],
                bottomLeft = [x - halfWidthDelta, y + halfHeight];
            return [topLeft, topRight, bottomRight, bottomLeft];
        };
        this.getAnglePosition = function (angleIndex) {
            var _this = this,
                position = _this.element.position,
                style = _this.element.style,
                shape = _this.element.shape,
                x = shape.x + position[0],
                y = shape.y + position[1],
                halfWidth = _this.width / 2,
                halfWidthDelta = (_this.width + _this.delta) / 2,
                halfHeight = _this.height / 2,
                delta = _this.delta;
            switch (angleIndex) {
                case 1:
                    x = x - halfWidthDelta + delta;
                    y = y - halfHeight;
                    break;
                case 2:
                    y = y - halfHeight;
                    break;
                case 3:
                    x = x + halfWidthDelta;
                    y = y - halfHeight;
                    break;
                case 4:
                    x = x + halfWidth;
                    break;
                case 5:
                    x = x + halfWidthDelta - delta;
                    y = y + halfHeight;
                    break;
                case 6:
                    y = y + halfHeight;
                    break;
                case 7:
                    x = x - halfWidthDelta;
                    y = y + halfHeight;
                    break;
                case 8:
                    x = x - halfWidth;
                    break;
            }
            return { x: x, y: y };
        };
        this.createPolygonOptions = function (selected) {
            var _this = this;
            var shapeOptions = _this.createActivityOptions();
            shapeOptions.shape.points = _this.buildPointList(_this.width, _this.height, _this.delta);
            shapeOptions.shape.smooth = "0.2";
            if (selected) shapeOptions.style.fill = _this.color.ACTIVITY_SELECTED_COLOR;
            return shapeOptions;
        };
        this.getMaxCoords = function () {
            var currentPosition = this.getCurrentPosition();
            return { x: currentPosition.x + (this.width + this.delta) / 2, y: currentPosition.y + this.height / 2 };
        };
        this.addTo = function (graph) {
            var _this = this,
                activityDataModel = graph.activityDataModel,
                shapeOptions = _this.createPolygonOptions(false);
            this.element = new Polygon(shapeOptions);
            graph.add(this.element);

            var data = new activityDataModel(_this);
            this.data = this.data ? util.merge(this.data, data) : data;
        };
        this.refresh = function () {
            var _this = this, shapeOptions = _this.createPolygonOptions(_this.selected);
            _this.element.setStyle(shapeOptions.style);
            return [_this.element];
        };
    }

    function NormalActivity(options) {
        Activity.apply(this, arguments);

        this.width = 120;
        this.height = 70;
        this.subtype = 2;
        this.getAnglePosition = function (angleIndex) {
            var _this = this,
                position = _this.element.position,
                style = _this.element.style,
                shape = _this.element.shape,
                x = shape.x + position[0],
                y = shape.y + position[1],
                halfWidth = _this.width / 2,
                halfHeight = _this.height / 2;
            var offsets = [[], [0, 0], [1, 0], [2, 0], [2, 1], [2, 2], [1, 2], [0, 2], [0, 1]];
            return {
                x: x + offsets[angleIndex][0] * halfWidth,
                y: y + offsets[angleIndex][1] * halfHeight
            };
        };
        this.createRectangleOptions = function (selected) {
            var _this = this,
                position = _this.position,
                shapeOptions = _this.createActivityOptions(),
                style = shapeOptions.style,
                shape = shapeOptions.shape;
            shape.x = position.x - _this.width / 2;
            shape.y = position.y - _this.height / 2;
            shape.width = _this.width;
            shape.height = _this.height;
            shape.r = 3;
            if (selected) shapeOptions.style.fill = _this.color.ACTIVITY_SELECTED_COLOR;
            return shapeOptions;
        };
        // 获取当前位置（因为可拖动，所以位置会变化）
        this.getCurrentPosition = function () {
            var _this = this,
                position = _this.element.position,
                style = _this.element.style,
                shape = _this.element.shape;
            return {
                x: shape.x + _this.width / 2 + position[0],
                y: shape.y + _this.height / 2 + position[1]
            };
        };
        this.getMaxCoords = function () {
            var currentPosition = this.getCurrentPosition();
            return { x: currentPosition.x + this.width / 2, y: currentPosition.y + this.height / 2 };
        };
        this.addTo = function (graph) {
            var _this = this,
                activityDataModel = graph.activityDataModel,
                shapeOptions = _this.createRectangleOptions(false);
            this.element = new Rectangle(shapeOptions);
            graph.add(this.element);

            var data = new activityDataModel(_this);
            this.data = this.data ? util.merge(this.data, data) : data;
        };
        this.refresh = function () {
            var _this = this,
                shapeOptions = _this.createRectangleOptions(_this.selected);
            _this.element.setStyle(shapeOptions.style);
            return [_this.element];
        };
    }

    function ActivitySwitch(options) {
        var activity;
        switch (options.subtype) {
            case 0:
                activity = new BothEndsActivity(options);
                break;
            case 1:
                activity = new SubFlowActivity(options);
                break;
            case 2:
                activity = new NormalActivity(options);
                break;
            default:
                break;
        }
        return activity;
    }

    function Transition(options) {
        Unit.apply(this, arguments);

        this.color = {
            // 连接弧线段颜色
            TRANSITION_STROKE_COLOR: '#999',
            // 连接弧已选中线段颜色
            TRANSITION_SELECTED_STROKE_COLOR: '#f63',
            // 连接弧固定点填充色
            TRANSITION_COLOR_FIXED: '#bbb',
            // 连接弧非固定点填充色
            TRANSITION_COLOR_UNFIXED: '#eee',
            // 连接弧（选中时）固定点填充色
            TRANSITION_SELECTED_COLOR_FIXED: '#f96',
            // 连接弧（选中时）非固定点填充色
            TRANSITION_SELECTED_COLOR_UNFIXED: '#fdc'
        };

        this.id = options.id;
        this.title = options.title;
        this.type = options.type || 'Transition';
        this.subtype = options.subtype || 0;

        this.from = options.from;
        // 起始活动角度（共八个角度）
        this.fromAngle = options.fromAngle;
        // 运行时，用于动态计算
        this.fromAngleRun = null;

        this.to = options.to;
        // 结束活动角度（共八个角度）
        this.toAngle = options.toAngle;
        this.toAngleRun = null;

        // 中间点
        this.middlePoints = options.middlePoints || [];

        this.polyline = null;
        this.arrow = null;
        this.circle = null;

        this.contextMenuItems = [];
        this.contextMenuItems.push(new ContextMenuExecuteItem({ text: '配置连线', action: 'setting', source: this }));
        this.contextMenuItems.push(new ContextMenuExecuteItem({ text: '删除连线', action: 'remove', source: this }));

        // 根据起止节点坐标，计算中间点的坐标
        this.calculteMiddlePointPositions = function (fromPosition, toPosition) {
            var length = Math.sqrt(Math.pow(fromPosition.x - toPosition.x, 2) + Math.pow(fromPosition.y - toPosition.y, 2));

            // 计算一个点
            function calcOne(middlePoint) {
                //
                var x1 = (toPosition.x - fromPosition.x) * middlePoint.h + fromPosition.x,
                    y1 = (toPosition.y - fromPosition.y) * middlePoint.h + fromPosition.y,
                    x2 = toPosition.x, y2 = toPosition.y,
                    a = length * middlePoint.v;
                if (middlePoint.h > 1) a = -a;
                var x = x1 - a * Math.sin(Math.atan2(y2 - y1, x2 - x1)),
                    y = y1 + a * Math.cos(Math.atan2(y2 - y1, x2 - x1));
                //console.debug(Object.toJSON([fromPosition.x,fromPosition.y,toPosition.x,toPosition.y, middlePoint.h, middlePoint.v, x1,y1, a, x, y]));
                return [x, y];
            }

            var results = [];
            this.middlePoints.each(function (middlePoint) { results.push(calcOne(middlePoint)); });
            return results;
        };

        // 创建线段配置项
        this.createPolylineOptions = function (selected) {

            var transition = this,
                graph = this.graph,
                oddAngleDelta = 15;
            // 找出目标活动中离参照点最近的角点
            function findShortestAngle(referencePosition, targetActivity) {
                var minAngleIndex = -1, minDistance = 0, minPosition = null;
                for (var angleIndex = 1; angleIndex <= 8; angleIndex++) {
                    var targetPosition = targetActivity.getAnglePosition(angleIndex);
                    distance = Math.sqrt(Math.pow(referencePosition.x - targetPosition.x, 2) + Math.pow(referencePosition.y - targetPosition.y, 2));
                    if (minAngleIndex === -1 || distance < minDistance || Math.abs(distance - minDistance) < oddAngleDelta && angleIndex % 2 === 0) {
                        // 差距不大时，偶数角点优先
                        minAngleIndex = angleIndex;
                        minDistance = distance;
                        minPosition = targetPosition;
                    }
                }
                return { angleIndex: minAngleIndex, distance: minDistance, position: minPosition };
            }

            // 找出两个活动中距离最近的两个角点
            function findShortestAngle2(activity1, activity2) {
                var minAngleIndex = -1, minDistance = 0, minPosition = null, angle2 = null;
                for (var angleIndex1 = 1; angleIndex1 <= 8; angleIndex1++) {
                    var activity1Position = activity1.getAnglePosition(angleIndex1);
                    var shortest = findShortestAngle(activity1Position, activity2);
                    if (minAngleIndex === -1 || shortest.distance < minDistance || Math.abs(shortest.distance - minDistance) < oddAngleDelta && angleIndex1 % 2 === 0) {
                        minAngleIndex = angleIndex1;
                        minDistance = shortest.distance;
                        minPosition = activity1Position;
                        angle2 = shortest;
                    }
                }
                return [{ angleIndex: minAngleIndex, position: minPosition, distance: minDistance }, angle2];
            }

            var fromActivity = graph.getActivity(transition.from),
                toActivity = graph.getActivity(transition.to),
                fromPosition, toPosition, result;
            if (transition.fromAngle < 1 || transition.fromAngle > 8) {
                if (transition.toAngle < 1 || transition.toAngle > 8) {
                    var results = findShortestAngle2(fromActivity, toActivity);
                    fromPosition = results[0].position;
                    transition.fromAngleRun = results[0].angleIndex;
                    toPosition = results[1].position;
                    transition.toAngleRun = results[1].angleIndex;
                } else {
                    toPosition = toActivity.getAnglePosition(transition.toAngle);
                    result = findShortestAngle(toPosition, fromActivity);
                    fromPosition = result.position;
                    transition.fromAngleRun = result.angleIndex;
                }
            } else {
                if (transition.toAngle < 1 || transition.toAngle > 8) {
                    fromPosition = fromActivity.getAnglePosition(transition.fromAngle);
                    result = findShortestAngle(fromPosition, toActivity);
                    toPosition = result.position;
                    transition.toAngleRun = result.angleIndex;
                } else {
                    fromPosition = fromActivity.getAnglePosition(transition.fromAngle),
                        toPosition = toActivity.getAnglePosition(transition.toAngle);
                }
            }

            var pointList;
            if (fromPosition.x === toPosition.x && fromPosition.y === toPosition.y) {
                // 起止节点相同时，长度和方向都没有意义，无法计算中间点
                pointList = [];
            } else {
                pointList = transition.calculteMiddlePointPositions(fromPosition, toPosition);
            }
            pointList.splice(0, 0, [fromPosition.x, fromPosition.y]);
            pointList.push([toPosition.x, toPosition.y]);
            //console.debug('>>>' + Object.toJSON(pointList));
            transition.choosePolylineStyle();

            return util.merge(transition.createShapeOptions(), {
                shape: {
                    points: pointList,
                    smooth: 'spline'
                },
                style: {
                    stroke: selected ? '#f63' : '#888',
                    lineWidth: transition.polylineStyleLineWidth,
                    lineDash: transition.polylineStyleLineDash,
                    text: transition.title,
                    fontFamily: graph.fontFamily,
                    textPosition: 'inside',
                    transformText: true
                },
                draggable: graph.designable,
                ondragstart: function (params) {
                    // 增加中间点，登记当前连接弧以及中间点
                    graph.onTransitionDragStart(transition, params.event);
                },
                ondragend: function (params) { graph.onTransitionDragEnd(params.event); }
            });
        };

        this.polylineStyleLineDash = [5, 5];
        this.polylineStyleLineWidth = 3;

        this.choosePolylineStyle - function () { };

        // 获取填充色
        this.getColor = function (selected, unfixed) {
            var _this = this,
                designable = this.graph.designable,
                isShowUnfixed = unfixed || !designable;
            return selected ? isShowUnfixed ? _this.color.TRANSITION_SELECTED_COLOR_UNFIXED : _this.color.TRANSITION_SELECTED_COLOR_FIXED : isShowUnfixed ? _this.color.TRANSITION_COLOR_UNFIXED : _this.color.TRANSITION_COLOR_FIXED;
        };

        // 创建箭头配置项（三角形）
        this.toAngleTargets = null;
        this.getToAngleTargets = function () { return this.toAngleTargets || (this.toAngleTargets = new AngleTargets(this.graph.getActivity(this.to), this, false)); };
        this.createArrowOptions = function (selected) {
            var transition = this,
                graph = this.graph,
                toAngleUnfixed = transition.toAngle < 1 || transition.toAngle > 8,
                pointList = transition.polyline.shape.points,
                fromPoint = pointList[pointList.length - 2], fromPosition = { x: fromPoint[0], y: fromPoint[1] },
                toPoint = pointList[pointList.length - 1], toPosition = { x: toPoint[0], y: toPoint[1] };

            // 箭头长度20，单边角度10度
            // 线段角度（从第四象限开始，顺时针旋转）
            var lineAngle = Math.atan2(fromPosition.x - toPosition.x, fromPosition.y - toPosition.y) / Math.PI * 180;
            var end1 = {
                x: toPosition.x + Math.sin(2 * Math.PI / 360 * (lineAngle - 15)) * 20,
                y: toPosition.y + Math.cos(2 * Math.PI / 360 * (lineAngle - 15)) * 20
            };
            var end2 = {
                x: toPosition.x + Math.sin(2 * Math.PI / 360 * (lineAngle + 15)) * 20,
                y: toPosition.y + Math.cos(2 * Math.PI / 360 * (lineAngle + 15)) * 20
            };

            return util.merge(this.createShapeOptions(), {
                shape: {
                    points: [[toPosition.x, toPosition.y], [end1.x, end1.y], [end2.x, end2.y]]
                },
                style: {
                    fill: transition.getColor(selected, toAngleUnfixed),
                    stroke: selected ? transition.color.TRANSITION_SELECTED_STROKE_COLOR : transition.color.TRANSITION_STROKE_COLOR,
                    lineWidth: 1,
                    text: ''
                },
                draggable: graph.designable,
                ondragstart: function (params) { transition.getToAngleTargets().start(graph); },
                ondragend: function (params) { transition.getToAngleTargets().end(graph); }
            });
        };

        this.fromAngleTargets = null;
        this.getFromAngleTargets = function () { return this.fromAngleTargets || (this.fromAngleTargets = new AngleTargets(this.graph.getActivity(this.from), this, true)); };
        this.createCircleOptions = function (selected) {
            var transition = this,
                graph = this.graph,
                fromActivity = graph.getActivity(transition.from),
                fromAngleUnfixed = transition.fromAngle < 1 || transition.fromAngle > 8,
                fromPosition = fromActivity.getAnglePosition(fromAngleUnfixed ? transition.fromAngleRun : transition.fromAngle);
            return util.merge(this.createShapeOptions(), {
                shape: {
                    cx: fromPosition.x,
                    cy: fromPosition.y,
                    r: 5
                },
                style: {
                    fill: transition.getColor(selected, fromAngleUnfixed),
                    stroke: selected ? transition.color.TRANSITION_SELECTED_STROKE_COLOR : transition.color.TRANSITION_STROKE_COLOR,
                    lineWidth: 1
                },
                draggable: graph.designable,
                ondragstart: function (params) { transition.getFromAngleTargets().start(graph); },
                ondragend: function (params) { transition.getFromAngleTargets().end(graph); }
            });
        };

        this.addTo = function (graph) {
            var transition = this,
                transitionDataModel = graph.transitionDataModel;

            // 线段
            var polylineOptions = transition.createPolylineOptions(false);
            polylineOptions.id += '-polyline';
            this.polyline = new Polyline(polylineOptions);
            graph.add(this.polyline);

            // 箭头
            var arrowOptions = transition.createArrowOptions(false);
            arrowOptions.id += '-arrow';
            this.arrow = new Polygon(arrowOptions);
            graph.add(this.arrow);

            // 起始圆点
            var circleOptions = transition.createCircleOptions(false);
            circleOptions.id += '-circle';
            this.circle = new Circle(circleOptions);
            graph.add(this.circle);

            var data = new transitionDataModel(transition);
            this.data = this.data ? util.merge(this.data, data) : data;
        };

        //删除图像
        this.remove = function () {
            this.graph.remove(this.polyline);
            this.graph.remove(this.arrow);
            this.graph.remove(this.circle);
            this.graph.removeTransition(this.id);
        };

        // 增加中间节点，并返回中间节点的索引
        this.addMiddlePoint = function (x, y) {
            var pointList = this.polyline.shape.points,
                pointListLength = pointList.length,
                insertIndex = -1;

            // 相邻两点方式查找插入点，同时也按最小相对距离方式进行查找
            var minLength = Number.MAX_VALUE, minLengthIndex = 0;
            for (var i = 0; i < pointListLength - 1; i++) {
                var point = pointList[i],
                    nextPoint = pointList[i + 1],
                    nextLength = Math.sqrt(Math.pow(point[0] - nextPoint[0], 2) + Math.pow(point[1] - nextPoint[1], 2)),
                    newLength = Math.sqrt(Math.pow(point[0] - x, 2) + Math.pow(point[1] - y, 2));
                if (newLength < nextLength) {
                    insertIndex = i;
                    break;
                } else {
                    if (newLength < minLength) {
                        minLength = newLength;
                        minLengthIndex = i;
                    }
                }
            }

            if (insertIndex === -1) insertIndex = minLengthIndex;

            // 计算并插入中间节点
            var fromPoint = pointList[0], toPoint = pointList[pointListLength - 1];
            this.middlePoints.splice(insertIndex, 0, _calcVH(fromPoint, toPoint, x, y));

            return insertIndex;
        };

        // 更新指定索引的中间节点值
        this.updateMiddlePoint = function (index, x, y) {
            var pointList = this.polyline.shape.points,
                fromPoint = pointList[0],
                toPoint = pointList[pointList.length - 1];
            var vh = _calcVH(fromPoint, toPoint, x, y);
            this.middlePoints[index] = vh;
            return this;
        };

        // 删除指定索引的中间点
        this.deleteMiddlePoint = function (index) {
            this.middlePoints.splice(index, 1);
            return this;
        };

        // 计算当前节点和直线的关系
        var _calcVH = function (fromPoint, toPoint, x, y) {
            // 直线方程：Ax + By + C = 0
            var x1 = fromPoint[0], x2 = toPoint[0],
                y1 = fromPoint[1], y2 = toPoint[1],
                A = y2 - y1,
                B = x1 - x2,
                C = x2 * y1 - x1 * y2;
            fromToLength = Math.sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)),
                fromXLength = Math.sqrt((x1 - x) * (x1 - x) + (y1 - y) * (y1 - y)),
                toXLength = Math.sqrt((x2 - x) * (x2 - x) + (y2 - y) * (y2 - y));
            var vLength = -(A * x + B * y + C) / Math.sqrt(A * A + B * B),
                hLength = Math.sqrt(fromXLength * fromXLength - vLength * vLength) * (toXLength > fromToLength ? -1 : 1);
            //console.debug([A, B, C, fromToLength, vLength, hLength, vLength/fromToLength, hLength/fromToLength]);
            return { v: vLength / fromToLength, h: hLength / fromToLength };
        };

        this.refresh = function () {
            var transition = this,
                graph = this.graph,
                results = [];

            var polylineOptions = transition.createPolylineOptions(transition.selected);
            var polyline = transition.polyline;
            polyline.setStyle(polylineOptions.style);
            polyline.setShape(polylineOptions.shape);
            results.push(polyline);

            var arrowOptions = transition.createArrowOptions(transition.selected);
            var arrow = transition.arrow;
            arrow.setStyle(arrowOptions.style);
            arrow.setShape(arrowOptions.shape);
            results.push(arrow);

            var circleOptions = transition.createCircleOptions(transition.selected);
            var circle = transition.circle;
            circle.setStyle(circleOptions.style);
            circle.setShape(circleOptions.shape);
            results.push(circle);

            // 创建中间点属性
            function buildMiddlePointOptions(middlePointIndex) {
                var point = pointList[middlePointIndex + 1];
                var dblclick = 0;
                var _dbltimer;
                return {
                    shape: {
                        cx: point[0],
                        cy: point[1],
                        r: 5
                    },
                    style: {
                        fill: transition.color.TRANSITION_SELECTED_COLOR_UNFIXED,
                        stroke: transition.color.TRANSITION_SELECTED_STROKE_COLOR,
                        lineWidth: 1
                    },
                    draggable: true,
                    ondblclick: function () {
                        transition.deleteMiddlePoint(middlePointIndex).refresh();
                    },
                    ondragstart: function (params) {
                        // 修改当前中间点，登记当前连接弧以及中间点
                        graph.onTransitionMiddlePointDragStart(transition, middlePointIndex);
                    },
                    ondragend: function (params) { graph.onTransitionMiddlePointDragEnd(); }
                };
            }
            if (transition.selected && transition.middlePoints.length > 0) {
                var pointList = transition.polyline.shape.points;
                transition.middlePointElements.each(function (element) { graph.remove(element); });
                transition.middlePointElements = [];
                transition.middlePoints.each(function (middlePoint, index) {
                    var middlePointElement = new Circle(buildMiddlePointOptions(index));
                    graph.add(middlePointElement);
                    transition.middlePointElements.push(middlePointElement);
                    results.push(middlePointElement);
                });
            } else {
                // 清除
                transition.middlePointElements.each(function (element) { graph.remove(element); });
                transition.middlePointElements = [];
            }

            return results;
        };

        this.middlePointElements = [];

        // 清除移动的位置
        this.clearMovement = function () {
            var transition = this;
            transition.polyline.position = [0, 0];
            transition.arrow.position = [0, 0];
            transition.circle.position = [0, 0];
            return transition;
        };
    }

    function ConditionalTransition() {
        Transition.apply(this, arguments);

        this.choosePolylineStyle = function () {
            if (this.subtype === 0) {
                //实线
                this.polylineStyleLineWidth = 3;
                this.polylineStyleLineDash = null;
            } else {
                //虚线
                this.polylineStyleLineWidth = 3;
                this.polylineStyleLineDash = [5, 5];
            }
        };
    }

    function TransitionSwitch(item) {
        var transition = new ConditionalTransition(item);
        return transition;
    }

    function ContextMenu() {
        this.zrClickPosition = { x: 0, y: 0 };

        // 显示上下文菜单
        this.showContextMenu = function (event, container, graph) {
            container.style.display = 'none';
            container.innerHTML = '';

            var ul = document.createElement('ul');
            container.appendChild(ul);
            this.buildContextMenuItems(ul, graph);

            // 加偏移，让鼠标位于菜单内
            var offset = -5;
            var rightEdge = event.target.clientWidth - event.clientX;
            var bottomEdge = event.target.clientHeight - event.clientY;
            if (rightEdge < container.offsetWidth)
                container.style.left = (event.target.scrollLeft + event.offsetX - container.offsetWidth + graph.offsetX + offset) + 'px';
            else
                container.style.left = (event.target.scrollLeft + event.offsetX + graph.offsetX + offset) + 'px';

            if (bottomEdge < container.offsetHeight)
                container.style.top = (event.target.scrollTop + event.offsetY - container.offsetHeight + graph.offsetY + offset) + 'px';
            else
                container.style.top = (event.target.scrollTop + event.offsetY + graph.offsetY + offset) + 'px';

            container.style.display = 'block';

            //记录画布点击坐标
            this.zrClickPosition = { x: event.zrX, y: event.zrY };
        };

        // 创建上下文菜单项
        this.buildContextMenuItems = function (container, graph) {
            this.contextMenuItems.forEach(function (item) {
                item.addTo(container);
            });
        };
    }

    function ContextMenuItem(options) {
        this.options = options;
        this.text = options.text;

        this.addTo = function (container) {
            var _this = this;
            var li = document.createElement('li');
            container.appendChild(li);

            var a = document.createElement('a');
            a.href = 'javascript:';
            li.appendChild(a);
            a.innerText = _this.text;
            a.onclick = function (event) {
                _this.onclick(event);
            };
        };
    }

    function ContextMenuExecuteItem(options) {
        ContextMenuItem.apply(this, arguments);

        this.action = options.action;
        this.unit = options.source;

        this.onclick = function (event) {
            var graph = this.unit.graph;
            switch (this.action) {
                case 'add':
                    var activity = {
                        id: graph.newGuid(),
                        subtype: 2,
                        position: { x: graph.zrClickPosition.x + 35, y: graph.zrClickPosition.y + 15 },
                        title: '新步骤'
                    };
                    designer.appendActivity([activity]);
                    break;
                case 'add2':
                    var activity2 = {
                        id: graph.newGuid(),
                        subtype: 1,
                        position: { x: graph.zrClickPosition.x + 45, y: graph.zrClickPosition.y + 40 },
                        title: '新子流程'
                    };
                    designer.appendActivity([activity2]);
                    break;
                case 'clear':
                    designer.clear();
                    break;
                case 'join':
                    graph.connectActivitys.clear().setType(1).add(this.unit);
                    break;
                case 'remove':
                    this.unit.remove();
                    break;
                case 'setting':
                    if (typeof graph.onUnitSetting === "function") {
                        graph.onUnitSetting(event, this.unit);
                    }
                    break;
                default:
                    console.log(this.unit);
                    break;
            }
        };
    }

    function Start(options) {
        options = util.merge({
            id: '00000000-0000-0000-0000-000000000001',
            subtype: 0,
            position: { x: 60, y: 60 },
            title: '开始',
            clickable: false
        }, options, true);
        BothEndsActivity.call(this, options);
    }

    function End(options) {
        options = util.merge({
            id: '00000000-0000-0000-0000-000000000002',
            subtype: 0,
            position: { x: 60, y: 60 },
            title: '结束',
            clickable: false
        }, options, true);
        BothEndsActivity.call(this, options);
    }

    function WheelTool() {
        this.position = { x: 0, y: 0 };
        this.width = 30;
        this.height = 30;

        this.createShapeOptions = function () {
            var _this = this;

            return {
                tag: 'tool',
                shape: {
                    width: _this.width,
                    height: _this.height,
                    r: 5
                },
                style: {
                    fill: "grey",
                    stroke: "#ddd",
                    lineWidth: 1,
                    textPosition: 'inside',
                    textFill: "#fff",
                    fontSize: 28,
                    rectHover: true
                }
            };
        };

        this.createEnlargedOptions = function () {
            var _this = this,
                shapeOptions = _this.createShapeOptions();

            return util.merge(shapeOptions, {
                shape: {
                    x: _this.position.x + 20,
                    y: _this.position.y + 20
                },
                style: {
                    text: "+"
                },
                onclick: function () {
                    _this.graph.handleWheel(true);
                }
            });
        };

        this.createNarrowOptions = function () {
            var _this = this,
                shapeOptions = _this.createShapeOptions();

            return util.merge(shapeOptions, {
                shape: {
                    x: _this.position.x + 20,
                    y: _this.position.y + 55
                },
                style: {
                    text: "-"
                },
                onclick: function () {
                    _this.graph.handleWheel(false);
                }
            });
        };

        this.addTo = function (graph) {
            this.graph = graph;

            var _this = this,
                enlargedOptions = _this.createEnlargedOptions(),
                narrowOptions = _this.createNarrowOptions();

            this.enlarged = new Rectangle(enlargedOptions);
            graph.add(this.enlarged);

            this.narrow = new Rectangle(narrowOptions);
            graph.add(this.narrow);
        };
    }

    function Designer() {
        var graph = new Graph();
        this.appendActivity = function (activities) {
            activities.forEach(function (item) {
                var activity = ActivitySwitch(item);
                activity.drawTo(graph);
            });
            return this;
        };
        this.appendTransition = function (transitions) {
            transitions.forEach(function (item) {
                var transition = TransitionSwitch(item);
                transition.drawTo(graph);
            });
            return this;
        };
        this.getData = function () {
            var activities = graph.getActivities().map(function (item) {
                return {
                    id: item.id,
                    title: item.title,
                    type: item.type,
                    subtype: item.subtype,
                    position: item.getCurrentPosition(),
                    data: item.data
                };
            });
            var transitions = graph.getTransitions().map(function (item) {
                return {
                    id: item.id,
                    title: item.title,
                    type: item.type,
                    subtype: item.subtype,
                    from: item.from,
                    fromAngle: item.fromAngle,
                    to: item.to,
                    toAngle: item.toAngle,
                    middlePoints: item.middlePoints,
                    data: item.data
                };
            });
            return {
                activities: activities,
                transitions: transitions
            };
        };
        this.getJson = function () {
            return JSON.stringify(designer.getData());
        };
        this.getZrenderInstance = function () {
            return graph.zrenderInstance();
        };
        this.init = function (options) {
            graph.init(options);
            return this;
        };
        this.clear = function () {
            graph.clear();
            var start = new Start({
                position: { x: 200, y: graph.zrenderInstance().getHeight() * 0.3 }
            });
            start.drawTo(graph);
            var end = new End({
                position: { x: graph.zrenderInstance().getWidth() - 200, y: graph.zrenderInstance().getHeight() * 0.3 }
            });
            end.drawTo(graph);

            return this;
        };
        this.load = function (data) {
            if (data) {
                graph.clear();
                if (data.activities) { this.appendActivity(data.activities); }
                if (data.transitions) { this.appendTransition(data.transitions); }
            }
            return this;
        };
        this.refresh = function () {
            graph.zrenderInstance().resize();
            graph.zrenderInstance().refresh();
        };
    }

    return new Designer();
}();