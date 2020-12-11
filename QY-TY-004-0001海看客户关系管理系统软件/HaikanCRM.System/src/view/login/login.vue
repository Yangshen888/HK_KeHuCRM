<style lang="less">
@import "./login.less";
</style>

<template>
  <div>
    <div class="bg bg-blur"></div>
    <div class="content content-front" v-if="isShow">
      <div class="login">
        <div class="login-con">
          <Card icon="log-in" title="欢迎登录" :bordered="false">
            <div class="form-con">
              <login-form
                @on-success-valid="handleSubmit"
                :processing="processing"
                :loading="loading"
              ></login-form>
            </div>
          </Card>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import LoginForm from "_c/login-form";
import { mapActions } from "vuex";
import axios from "@/libs/api.request";
import store from "@/store";
import { initRouter } from "@/libs/router-util";

const getUserList = (data) => {
  return axios.request({
    url: 'Oauth/UnifyAuth?unifyToken='+data,
    method: 'get',
    
  })
}

export default {
  components: {
    LoginForm
  },
  data() {
    return {
      processing: false,
      loading: false,
      isShow:false,
    };
  },
  mounted(){
    
    console.log(22222);
    console.log(this.$route);
    let utoken=this.$route.query.token;
    if(typeof(this.$route.query.token) != "undefined"){
      this.isShow=false;
      getUserList(utoken).then(res=>{
        console.log(res);
        if (res.data.code == 200) {
            this.processing = true;
            localStorage.setItem("token", res.data.data);
            this.$Message.loading({
              duration: 0,
              closable: false,
              content: "用户信息验证成功,正在登录系统..."
            });
            localStorage.setItem('xxxx',"0")
            this.getUserInfo().then(res => {
              setTimeout(() => {
                initRouter(this);
                this.$router.push({
                  name: "home"
                });

                setTimeout(() => {
                  this.$Message.destroy();
                }, 1);
              }, 10);
            });
          } else {
            localStorage.setItem('xxxx','0');
            this.processing = false;
            this.loading = false;
            this.isShow=true;
            this.$Message.error(res.data.message);
            
          }
      });
    }else{
      this.isShow=true;
    }
    
  },
  methods: {
    ...mapActions(["handleLogin", "getUserInfo"]),
    handleSubmit({ userName, password }) {
      console.log("页面启动调用");
      var target = this;
      this.loading = true;
      this.handleLogin({ userName, password })
        .then(res => {
          if (res.data.code == 200) {
            this.processing = true;
            this.$Message.loading({
              duration: 0,
              closable: false,
              content: "用户信息验证成功,正在登录系统..."
            });
            localStorage.setItem('xxxx',"0")
            this.getUserInfo().then(res => {
              setTimeout(() => {
                initRouter(target);
                this.$router.push({
                  name: "home"
                });

                setTimeout(() => {
                  this.$Message.destroy();
                }, 100);
              }, 150);
            });
          } else {
            this.processing = false;
            this.loading = false;
            this.$Message.error(res.data.message);
          }
        })
        .catch(error => {
          target.loading = false;
          if (!error.status) {
            this.$Message.error({
              content: "网络出错,请检查你的网络或者服务是否可用",
              duration: 5
            });
          }
        });
    }
  }
};
</script>

<style>
.demo-spin-icon-load {
  animation: ani-demo-spin 1s linear infinite;
}
.content {
  color: #ffffff;
  font-size: 40px;
}
.bg {
  background: url("../../assets/images/海看CRM登录背景.png");
  height: 100%;
  text-align: center;
  line-height: 100%;
  position: absolute;
}
.bg-blur {
  float: left;
  width: 100%;
  background-repeat: no-repeat;
  background-position: center;
  background-size: cover;
}
.content-front {
  position: absolute;
  left: 10px;
  right: 10px;
  height: 100%;
  line-height: 100%;
}
</style>
