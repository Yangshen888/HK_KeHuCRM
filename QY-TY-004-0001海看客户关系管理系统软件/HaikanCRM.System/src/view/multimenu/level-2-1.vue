<template>
  <div>
    <div>
      <ckeditor
        id="editor"
        :editor="editor"
        @ready="onReady"
        v-model="content"
        :config="editorConfig"
      ></ckeditor>
    </div>
  </div>
</template>
<script>
import { getToken } from "@/libs/util";
import MyUploadAdapter from "../../global/MyUploadAdapter.js";
import DecoupledEditor from "@ckeditor/ckeditor5-build-decoupled-document";
import PasteFromOffice from '@ckeditor/ckeditor5-paste-from-office/src/pastefromoffice';
// import SimpleUploadAdapter from "@ckeditor/ckeditor5-upload/src/adapters/simpleuploadadapter";
import "@ckeditor/ckeditor5-build-decoupled-document/build/translations/zh-cn.js";
export default {
  name: "level_2_1",
  
  data() {
    return {
      content: "<p>Content of the editor.</p>",
      editor: DecoupledEditor,
      editorConfig: {
        language: "zh-cn",
        // simpleUpload: {
        //   uploadUrl:
        //     "http://192.168.0.226:4062/api/v1/common/common/UpLoadPicture", // 你的上传图片地址
        //   headers: {
        //     Authorization: "Bearer " + getToken(),
        //   },
        // },
        // The configuration of the editor.
        plugins: [PasteFromOffice]
      },
    };
  },
  methods: {
    onReady(editor) {
      // Insert the toolbar before the editable area.

      editor.ui
        .getEditableElement()
        .parentElement.insertBefore(
          editor.ui.view.toolbar.element,
          editor.ui.getEditableElement()
        );
      editor.plugins.get("FileRepository").createUploadAdapter = (loader) => {
        return new MyUploadAdapter(loader);
      };
      editor.plugins
      // DecoupledEditor.create(document.querySelector("#editor"), {
      //   extraPlugins: [MyUploadAdapter],

        // ...
      // }).catch((error) => {
      //   console.log(error);
      // });
    },
  },
};
</script>
