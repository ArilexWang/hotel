//点击事件类
function ClickObj(){

}

ClickObj.prototype = {
    constructor: ClickObj,
    //导航栏收缩展开按钮
    collapseClick: function(){
        var eDom = $('.messageBox');
        //收缩导航栏
        if(eDom.css("display") != "none"){
            eDom.animate({left:"-=240px"},300);
            $(this).animate({left:"30px"},400);
            eDom = $(this).children().children().first();
            eDom.css({transform: " rotate(135deg)"})
            eDom = eDom.parent().children().last();
            eDom.css({transform: " rotate(-135deg)"})
            $('.messageBox').toggle();
        }
        //打开导航栏
        else{
            eDom.animate({left:"+=240px"},300);
            $(this).animate({left:"180px"},400);
            eDom = $(this).children().children().first();
            eDom.css({transform: " rotate(44deg)"})
            eDom = eDom.parent().children().last();
            eDom.css({transform: " rotate(-44deg)"})
            $('.messageBox').toggle();
        }    
    }

    
}

//点击事件实体
var clickobj = new ClickObj();




$(document).ready(function(){
    
    $(".collapseBtn").click(clickobj.collapseClick)
})