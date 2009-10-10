﻿/// <reference path="../jquery-vsdoc.js" />
function addFav() {
    if (_infos.l) {
        showPage({
            title: '加为收藏',
            bgColor: '#888',
            marginTop: 100,
            url: relativeUrl + 'pages/profiles/AddFav.aspx?type=p&&id=' + _infos.d + "&&t=" + Math.random()
        });
    }
    else {
        _showMsg('登录用户才能进行收藏操作!');
    }
}
function _showMsg(msg) {
    showMsg({
        top: 150,
        bgColor: '#888',
        msg: msg
    });
}
function addCar() {
    var v = $.trim($('#txtAmount').val());
    var t = $('#anchorAddCar');
    if (isNaN(v)) {
        return;
    }
    else {
        t.removeClass('addcar').addClass('addload');
    }
}
function checkAmount() {
    var v = $.trim(this.value);
    if (isNaN(v)) {
        if (!$$ErrorMsg.is(':visible')) {
            $$ErrorMsg.show();
        }
    }
    else {
        if ($$ErrorMsg.is(':visible')) {
            $$ErrorMsg.hide();
        }
    }
}
$().ready(function() {
    var pics = new Array();
    $.each(pictures, function(i, n) {
        pics.push({
            Link: n.Url,
            Url: n.Url,
            Title: '',
            Description: ''
        })
    });
    if (pics.length != 0 && pics[0].Url != '') {
        $('#divPics').hrzAccordion({
            pictures: pics,
            width: 300,
            height: 200,
            titleHeight: 0
        });
    }
    else {
        $('#divPics').html('没有展示图片！');
    }
    window.$$ErrorMsg = $('#spnMsg');
    $('#anchorAddFav').click(addFav);
    $('#anchorAddCar').click(addCar);
    $('#txtAmount').keyup(checkAmount);
});