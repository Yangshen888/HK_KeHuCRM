export default {
  /**
   * @description 配置显示在浏览器标签的title
   */
  title: window.configData.title,
  /**
   * @description token在Cookie中存储的天数，默认1天
   */
  cookieExpires: 1,
  /**
   * @description 是否使用国际化，默认为false
   *              如果不使用，则需要在路由中给需要在菜单中展示的路由设置meta: {title: 'xxx'}
   *              用来在菜单中显示文字
   */
  useI18n: false,
  /**
   * @description api请求基础路径
   */
  baseUrl: {
    // dev: 'http://192.168.0.221:4449/',
    // pro: 'http://192.168.0.221:4449/',
    // dev: 'https://crm.api.dodokon.com/',
    // pro: 'https://crm.api.dodokon.com/',
    // dev: 'http://localhost:5001/',
    // pro: 'http://localhost:5001/',
    // dev: 'http://localhost:54321/',
    // pro: 'http://localhost:54321/',
    // dev: 'https://api.crm.haikan.com.cn/',
    // pro: 'https://api.crm.haikan.com.cn/',
    dev: window.configData.baseUrl.dev,
    pro: window.configData.baseUrl.pro,
    // defaultPrefix: "api/v1/"
    defaultPrefix: window.configData.baseUrl.defaultPrefix
  },
  authUrl: {
    // dev: 'http://192.168.0.221:4449/api/oauth/auth',
    // pro: 'http://192.168.0.221:4449/api/oauth/auth',
    // dev: 'https://crm.api.dodokon.com/api/oauth/auth',
    // pro: 'https://crm.api.dodokon.com/api/oauth/auth',
    // dev: 'http://localhost:5001/api/oauth/auth',
    // pro: 'http://localhost:5001/api/oauth/auth'
    // dev: 'http://localhost:54321/api/oauth/auth',
    // pro: 'http://localhost:54321/api/oauth/auth'
    // dev: 'https://api.crm.haikan.com.cn/api/oauth/auth',
    // pro: 'https://api.crm.haikan.com.cn/api/oauth/auth',
    dev: window.configData.authUrl.dev,
    pro: window.configData.authUrl.pro,
  },
  /**
   * @description 默认打开的首页的路由name值，默认为home
   */
  homeName: 'home',
  /**
   * @description 需要加载的插件
   */
  plugin: {
    'error-store': {
      showInHeader: true, // 设为false后不会在顶部显示错误日志徽标
      developmentOff: true // 设为true后在开发环境不会收集错误信息，方便开发中排查错误
    }
  },
  // webUrl:'http://localhost:9000',
  //webUrl:'http://crm.haikan.com.cn',
  webUrl: window.configData.webUrl,
  
  haikanPassport_IfUse:window.configData.HaikanPassport_IfUse,




}
