<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" v-on:click="back">
			<block slot="backText">返回</block>
			<block slot="content">联系内容详情</block>
		</cu-custom>
	<form>
<!-- 			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">联系人:</view>
				<input name="input" v-model="mobile.contactName" disabled="don"  style="text-align:right;font-size: 15px; "></input>
			</view>

			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">客户:</view>
				<input name="input" v-model="mobile.clientName" disabled="don"  style="text-align:right;font-size: 15px; "></input>
			</view> -->		
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">联系时间:</view>
				<input name="input" v-model="mobile.callTime" disabled="don"  style="text-align:right ;font-size: 15px;"></input>
			</view>
			<view class="cu-form-group align-start">
				<view class="title" style="font-size: 15px;">联系内容:</view>
				<textarea maxlength="-1" @input="textareaBInput" v-model="mobile.callContent" disabled="don"  style="text-align:right;font-size: 15px; "></textarea>
			</view>
		</form>
	</view>
</template>

<script>
	import uniPopup from '@/components/uni-popup/uni-popup.vue';
	import uniIcon from '@/components/uni-icon/uni-icon.vue';
	import {
		AppCallLogInfo,
	} from '@/api/clien/calllog.js';
	export default {
		components: {
			uniIcon,
			uniPopup
		},
		data() {
			return {
							don: "on",
				InputBottom: 0,
				stores: {
					pogoda: {
						logUuid: "",
						id: 0,
						contactName: "",
						SystemUserUuid: ''
					}
				},
				mobile: {
					callLogUuid:"",
					contactName: "",
					clientName: "",
					callTime: "",
					callContent: "",
				},
				kw: [],
			}
		},
		methods: {
			back() {
				this.$router.go(-1);
			},

			InputFocus(e) {
				this.InputBottom = e.detail.height
			},
			InputBlur(e) {
				this.InputBottom = 0
			},

		},
		onLoad(opt){
			console.log(opt);
			let data=opt.callLogUuid;
			console.log(data);
				AppCallLogInfo({callLogUuid:data}).then(res=>{
				console.log(res);
				this.mobile = res.data.data[0];
			});
		},
	}
</script>

<style>
	.container999 {
		width: 100vw;
		font-size: 28upx;
		/* min-height: 100vh; */
		overflow: hidden;
		color: #6b8082;
		position: relative;
		/* top:50px; */
		background-color: #f6f6f6;
	}

	.card {
		width: 90%;
		background-color: white;
		margin: 40rpx auto 42upx auto;
		background: #ffffff;
		box-shadow: 0 0 5px 0 rgba(0, 0, 0, 0.1);
		border-radius: 5px;
		position: relative;
		padding: 40upx;
		line-height: 50upx;
	}

	.viewcontent {
		text-indent: 2em;
		line-height: 50upx;
		padding: 20rpx;
	}

	.tt {
		font-size: 30upx;
		font-weight: bold;
		width: 100px;
	}

	.bg {
		background-image: url(@/static/image/bg-lan.png);
		background-position: 50%;
		background-size: cover;
		box-shadow: 0 0 1px 0 rgba(0, 0, 0, 0.1);
	}

	.cu-card.article>.cu-item .title {
		line-height: 80rpx;
		color: #FFFFFF;
		font-weight: 400;
	}

	.cu-items {
		margin: 30rpx;
	}

	.cu-list.menu-avatar>.cu-item .content {
		left: 30rpx;
		line-height: 60rpx;
	}

	.cu-list.menu-avatar>.cu-item {
		height: 280rpx;
	}

	.youshang {
		position: absolute;
		top: 15px;
		right: 15px;
	}

	.youxia {
		position: absolute;
		bottom: 15px;
		right: 15px;
	}

	.youshang img {
		width: 30rpx;
		height: 30rpx;
	}

	.youxia img {
		width: 40rpx;
		height: 40rpx;
	}
</style>
