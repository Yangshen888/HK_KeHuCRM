<template>
  <div style="text-align:center;background-color: #fff;">
    <div style="display: inline-block;width:500px;">
      <div style="width:100%;display:flex;height:50px;">
        <div style="width:80px;line-height:50px;">
          <label style="color: red">*</label>
          <label style="font-size: 15px">系统名称</label>
        </div>
        <Input
          v-model="Global.clobalName"
          style="margin: 10px 0 10px 20px; width: 400px"
          placeholder="请输入系统名称"
        />
      </div>
      <div>
        <fieldset style="width: 100%">
          <legend class="legend" style="font-size: 15px">logo图片</legend>
          <div style="padding: 15px;height:400px;">
            <div class="demo-upload-list" v-for="item in uploadList">
              <div v-if="item.status === 'finished'">
                <img :src="item.url" style="width:250px;height:200px; object-fit:contain;" />
              </div>
              <div v-else>
                <Progress
                  v-if="item.showProgress"
                  :percent="item.percentage"
                  hide-info
                ></Progress>
              </div>
            </div>
            <Upload
              ref="upload"
              :show-upload-list="false"
              :default-file-list="defaultList"
              :on-success="showUpResult"
              :on-progress="toUpResult"
              :format="['jpg', 'jpeg', 'png']"
              :max-size="5120"
              :data="{ fileSavePath: 'Zhpt/Qybj' }"
              :on-format-error="handleFormatError"
              :on-exceeded-size="handleMaxSize"
              type="drag"
              :action="actionurl"
              style="display: inline-block;margin:40px 0;"
            >
              <div style="width: 100px; height: 100px; line-height: 100px">
                <Icon type="ios-camera" size="20"></Icon>
              </div>
            </Upload>
          </div>
        </fieldset>
      </div>
      <div>
        <Button style="margin: 50px;width:150px;" type="primary" @click="submintSave"
          >确定</Button
        >
      </div>
    </div>
  </div>
</template>
<script>
import config from "@/config";
import { getGlobalList, editGlobalList } from "@/api/rbac/global";
export default {
 name: "global",
  data() {
    return {
      uploadList: [],
      defaultList: [],
      actionurl: config.baseUrl.dev + "api/v1/rbac/Global/Upload",
      imgName: "",
      Global: {
        clobalUuid: "",
        clobalName: "", //全局名称
        globalUrl: "", //全局背景
        globalLogo: "", //全局logo
        addPeople: "",
        addTime: "",
        isDeleted: "",
      },
    };
  },
  //初始化
  mounted() {
    getGlobalList().then((res) => {
      console.log(res);
      this.Global = res.data.data[0];
      console.log(this.Global.clobalName);
      this.uploadList.push({
          url: config.baseUrl.dev + this.Global.globalLogo,
          status: "finished",
          name: this.Global.globalLogo,
          fileName:this.Global.globalLogo,
        });
    });
  },
  methods: {
    submintSave() {
      console.log(this.Global);
      editGlobalList(this.Global).then((res) => {
        this.$Message.success("上传成功！");
      });
       location.reload(true);
    },
    getpath(path) {
      if (path == "undefined" || path == null || path == "") {
        return;
      }
      return config.baseUrl.dev + path.replace("\\", "/");
    },
    async showUpResult(response, file, fileList) {
      console.log("图片");
      console.log(
        config.baseUrl.dev + response.data.strpath.replace("\\", "/")
      );
      this.loadingStatus = false;
      this.updisabled = false;
      if (response.code == "200") {
        this.$Message.success(response.message);
        this.Global.globalLogo = response.data.strpath;
        this.uploadList=[];
        await this.uploadList.push({
          url: config.baseUrl.dev + response.data.strpath.replace("\\", "/"),
          status: "finished",
          name: response.data.strpath,
          fileName: response.data.strpath,
        });
       
      } else {
        this.$Message.warning(response.message);
      }
    },
    toUpResult() {
      if (this.$refs.upload.fileList.length > 1) {
        this.$refs.upload.fileList.shift();
      }
      this.loadingStatus = true;
      this.updisabled = true;
    },
    handleFormatError(file) {
      this.$Notice.warning({
        title: "The file format is incorrect",
        desc: "文件不是png,jpg格式",
      });
    },
    handleMaxSize(file) {
      this.$Notice.warning({
        title: "Exceeding file size limit",
        desc: "文件大小不超过5M.",
      });
    }
  },
};
</script>
<style>
img{
  width: 85px;
  height: 60px;
}
</style>