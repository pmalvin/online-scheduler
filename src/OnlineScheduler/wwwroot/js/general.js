/*这是该项目的默认js文件
 *各个页面也会有自己的js文件
 */
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

var sideVm = new Vue({
    el: '#side-bar'
});