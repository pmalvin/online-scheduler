/*这是该项目的默认js文件
 *各个页面也会有自己的js文件
 */
var sideBarVm = new Vue({
    el: '#side-bar',
    data: {
        page: currentPage //定义在了layout的head部分
    },
    computed: {
        selectedItem: function () {
            return this.page;
        }
    },
});

var headerVm = new Vue({
    el: '#header',
    data: {
        isLoggedIn: false,
        userName: ''
    },
    methods: {
        toggleLoggedIn: function () {

        }
    }
});