var timetable = new Vue({
    el: '#timetable-page',
    created: function () {
        this.currentDate = new Date();
        getPlansForDate(this.currentDate);
    },
    data: {
        currentDate: undefined,
        plans: [],
    },
    methods: {
        toNextDay: function () {
            this.currentDate = new Date(Number(this.currentDate) + 1000 * 60 * 60 * 24);
            refresh();
        },
        toLastDay: function () {
            this.currentDate = new Date(this.currentDate - 1000 * 60 * 60 * 24);
            refresh();
        },
        smartFormatDate: function (dateStr) {
            var date = new Date(dateStr);
            if (date.toDateString() == this.currentDate.toDateString()) {
                return date.toLocaleTimeString('zh-CN', { hour12: false });
            } else if (this.isToday) {
                var near = this.daysNear[date.toDateString()];
                if (near) {
                    return near;
                } else {
                    return date.toLocaleDateString();
                }
            } else {
                return date.toLocaleDateString();
            }
        },
        edit: function (plan) {

        },
        toggleFinish: function (plan) {
            console.log(plan.isFinished + 'to' + !plan.isFinished);
            $.ajax({
                url: '/api/plan/' + plan.planId,
                method: 'PATCH',
                data: String(!plan.isFinished),
                contentType: 'application/json',
                success: function (data) {
                    plan.isFinished = !plan.isFinished;
                },
                error: function (err) {
                    console.log(err);
                }
            })
        },
        deletePlan: function (plan) {
            /*在这之前要先弹一个modal问询*/
            $.ajax({
                url: '/api/plan/' + plan.planId,
                method: 'DELETE',
                success: function (data) {
                    timetable.plans.splice(timetable.plans.indexOf(plan), 1);
                },
                error: function (err) {
                    console.log(err);
                }
            });
        }
    },
    computed: {
        dateTitle: function () {
            return formatDate(this.currentDate);
        },
        isToday: function () {
            return this.currentDate.toDateString() == new Date().toDateString();
        },
        daysNear: function () {
            var dict = {};
            var today = new Date();
            dict[new Date(Number(today) + 1000 * 3600 * 24).toDateString()] = '明天';
            dict[new Date(Number(today) + 1000 * 3600 * 48).toDateString()] = '后天';
            dict[new Date(Number(today) - 1000 * 3600 * 24).toDateString()] = '昨天';
            dict[new Date(Number(today) - 1000 * 3600 * 48).toDateString()] = '前天';
            return dict;
        }

    }
});
function formatDate(dateobj) {
    var dayDict = ['日', '一', '二', '三', '四', '五', '六'];
    var year = dateobj.getFullYear();
    var month = dateobj.getMonth() + 1;
    var day = dayDict[dateobj.getDay()];
    var date = dateobj.getDate();
    return year + '年' + month + '月' + date + '日 星期' + day;
}
function refresh() {
    getPlansForDate(timetable.currentDate);
}
function getPlansForDate(date) {
    $.ajax({
        url: '/api/plan?date=' + date.toLocaleDateString(),
        method: 'GET',
        success: function (data) {
            timetable.plans = data;
        },
        error: function (error) {
            console.log(JSON.stringify(error));
        }
    });
}