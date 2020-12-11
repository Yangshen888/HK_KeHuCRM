window.configData = {
    // "title":"海看客户关系管理",
    // 配置显示在浏览器标签的title
    "title": "海看客户关系管理",
    //token在Cookie中存储的天数，默认1天
    "cookieExpires": 1,
    //是否使用国际化，默认为false如果不使用，则需要在路由中给需要在菜单中展示的路由设置meta: {title: 'xxx'}用来在菜单中显示文字
    "useI18n": false,
    //默认打开的首页的路由name值，默认为home
    "homeName": 'home',
    //api请求基础路径
    "baseUrl": {
        // "dev": "http://localhost:54321/",
        // "pro": "http://localhost:54321/",
        "dev": 'https://api.crm.haikan.com.cn/',
        "pro": 'https://api.crm.haikan.com.cn/',
        "defaultPrefix": "api/v1/"
    },
    "authUrl": {
        // "dev": 'http://localhost:54321/api/oauth/auth',
        // "pro": 'http://localhost:54321/api/oauth/auth'
        "dev": 'https://api.crm.haikan.com.cn/api/oauth/auth',
        "pro": 'https://api.crm.haikan.com.cn/api/oauth/auth'
    },
    //需要加载的插件
    "plugin": {
        'error-store': {
            "showInHeader": true, // 设为false后不会在顶部显示错误日志徽标
            "developmentOff": true // 设为true后在开发环境不会收集错误信息，方便开发中排查错误
        }
    },
    //统一身份认证回调路径
    // "webUrl": 'http://localhost:9000',
    "webUrl": 'http://crm.haikan.com.cn',

    //是否启用统一身份认证
     "HaikanPassport_IfUse": false,
    // "HaikanPassport_IfUse":true,

}