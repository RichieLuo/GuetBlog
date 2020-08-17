


$('.admin-navs li.nav-item>a').off('click').on('click', function () {
    let _$this = $(this);
    let _name = _$this.text();
    $('#BlogNavName').text(_name);
    $('.nav-subnav li a').removeClass('nav-active');
     BlogMenu.set({
       id: _$this.data('id'),
        name:_name,
        href: _$this.data('href')      
    });
    console.log(BlogMenu.get());
});

$('.admin-navs li.nav-item-has-subnav ul.nav-subnav li>a').off('click').on('click', function () {
    let _$this = $(this);
    let _name = _$this.text();
    $('#BlogNavName').text(_name);

    $('.nav-subnav li a').removeClass('nav-active');
    _$this.addClass('nav-active');
    BlogMenu.set({
        id: _$this.data('id'),
        name:_name,
        href: _$this.data('href')         
    });

    console.log(BlogMenu.get());
});


let BlogMenu = {
    get: function () {
        return JSON.parse(localStorage.getItem('BlogMenu')) || {id:'Index', name: '后台首页', href:'/Admin/Index' };
    },
    set: function (menu) {
        localStorage.setItem('BlogMenu', JSON.stringify(menu));
    }
};