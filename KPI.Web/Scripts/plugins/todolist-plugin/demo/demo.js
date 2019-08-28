$(function () {
    //Custom datepicker
    $('#todo-lists-demo-datepicker').lobiList({
     
      
        init: function () {
            //console.log("Da tao TODO");
            //console.log(arguments);
            //console.log(this.$options);
        },
        // Default options for all lists
        listsOptions: {
            id: false,
            title: '',
            items: []
        },
        // Default options for all todo items
        itemOptions: {
            id: false,
            title: '',
            description: '',
            dueDate: '',
            done: false
        },
        lists: [],
        // Whether to show checkboxes or not
        useCheckboxes: true,
        // Show/hide todo remove button
        enableTodoRemove: true,
        // Show/hide todo edit button
        enableTodoEdit: true,
        // Whether to make lists and todos sortable
        sortable: true,
        // Default action buttons for all lists
        controls: ['edit', 'add', 'remove', 'styleChange'],
        //List style
        defaultStyle: 'lobilist-info',
        // Whether to show lists on single line or not
        onSingleLine: true,
        beforeDestroy: function () {
            //console.log("Da tao TODO");
        },
        afterDestroy: function () {

        },
        beforeListAdd: function () {
            
        },
        afterListAdd: function (lobilist, list) {
            //console.log("Console lobilist");
            //console.log(lobilist);
            //console.log("Console list");
           
        },
        beforeListRemove: function () {

        },
        afterListRemove: function () {

        },
        beforeItemAdd: function () {
           
        },
        afterItemAdd: function (lobilist,list) {
            console.log("Click add");
            console.log(lobilist.$options.id);
            $.post('Add', { item: arguments[1] }, function (data) {

            });
        },
        beforeItemUpdate: function () {

        },
        afterItemUpdate: function () {

        },
        beforeItemDelete: function () {

        },
        afterItemDelete: function () {

        },
        beforeListDrop: function () {

        },
        afterListReorder: function () {

        },
        beforeItemDrop: function () {

        },
        afterItemReorder: function () {

        },
        afterMarkAsDone: function () {

        },
        afterMarkAsUndone: function () {

        },
        styleChange: function (list, oldStyle, newStyle) {

        },
        titleChange: function (list, oldTitle, newTitle) {
            //console.log(list);
            //console.log(oldTitle);
            //console.log(newTitle);
        },
           actions: {
            'load': '/ChartPeriod/LoadToDo',
            'update': '',
            'insert': '',
            'delete': ''
        }
    });

    //get the LobiList instance
    var $instance = $('#todo-lists-demo-datepicker').data('lobiList');
    //call the methods
    $instance.addList({
        title: 'To Do list',
        defaultStyle: 'lobilist-info',
        useCheckboxes: true,
        item: []
    });

    console.log($instance.$lists);
    //var list = $('#todo-lists-demo-events')
    //        .lobiList({
    //            init: function () {

    //            },
    //            beforeDestroy: function () {

    //            },
    //            afterDestroy: function () {

    //            },
    //            beforeListAdd: function () {

    //            },
    //            afterListAdd: function () {

    //            },
    //            beforeListRemove: function () {

    //            },
    //            afterListRemove: function () {

    //            },
    //            beforeItemAdd: function () {

    //            },
    //            afterItemAdd: function () {
    //                console.log(arguments[1]); 
    //            },
    //            beforeItemUpdate: function () {

    //            },
    //            afterItemUpdate: function () {

    //            },
    //            beforeItemDelete: function () {

    //            },
    //            afterItemDelete: function () {

    //            },
    //            beforeListDrop: function () {

    //            },
    //            afterListReorder: function () {

    //            },
    //            beforeItemDrop: function () {

    //            },
    //            afterItemReorder: function () {

    //            },
    //            afterMarkAsDone: function () {

    //            },
    //            afterMarkAsUndone: function () {

    //            },
    //            styleChange: function (list, oldStyle, newStyle) {

    //            },
    //            titleChange: function (list, oldTitle, newTitle) {

    //            },
    //            lists: [
    //                {
    //                    title: 'TODO',
    //                    defaultStyle: 'lobilist-info',
    //                    items: [

    //                    ]
    //                }
    //            ]
    //        })
    //    .data('lobiList');


});