<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" v-on:click="back">
			<block slot="backText">返回</block>
			<block slot="content">联系人信息</block>
		</cu-custom>
		<view class="cu-card article">
			<!-- :class="isCard?'no-card':''" -->
			<form>
				<view class="cu-form-group">
					<view class="title" style="font-size: 15px;">联系人名称:</view>
					<input name="input" v-model="mobile.contactName" disabled="don" style="text-align:right;font-size: 15px;  "></input>
				</view>
				<view class="cu-form-group">
					<view class="title" style="font-size: 15px;">所属客户:</view>
					<input name="input" v-model="mobile.clientName" disabled="don" style="text-align:right;font-size: 15px;  "></input>
				</view>
				<!-- 				<view class="cu-form-group">
					<view class="title">头衔:</view>
					<input name="input" v-model="mobile.title" disabled="don" style="text-align:right "></input>
				</view> -->
				<view class="cu-form-group">
					<view class="title" style="font-size: 15px;">职务:</view>
					<input name="input" v-model="mobile.call" disabled="don" style="text-align:right;font-size: 15px;  "></input>
				</view>
				<view class="cu-form-group">
					<view class="title" style="font-size: 15px;">电话:</view>
					<input name="input" v-model="mobile.phone" disabled="don" style="text-align:right;font-size: 15px;  "></input>
				</view>
				<view class="cu-form-group">
					<view class="title" style="font-size: 15px;">邮件:</view>
					<input name="input" v-model="mobile.email" disabled="don" style="text-align:right;font-size: 15px;  "></input>
				</view>
				<view class="cu-form-group">
					<view class="title" style="font-size: 15px;">性别:</view>
					<input name="input" v-model="mobile.sex" disabled="don" style="text-align:right;font-size: 15px;  "></input>
				</view>
				<view class="cu-form-group">
					<view class="title" style="font-size: 15px;">生日:</view>
					<input name="input" v-model="mobile.dateBirth" disabled="don" style="text-align:right;font-size: 15px;  "></input>
				</view>
				<view class="cu-form-group">
					<view class="title" style="font-size: 15px;">微信:</view>
					<input name="input" v-model="mobile.weChat" disabled="don" style="text-align:right;font-size: 15px;  "></input>
				</view>
				<view class="cu-form-group">
					<view class="title" style="font-size: 15px;">QQ:</view>
					<input name="input" v-model="mobile.tencentQq" disabled="don" style="text-align:right;font-size: 15px;  "></input>
				</view>
				<!-- 				<view class="cu-form-group">
					<view class="title">手机号码:</view>
					<input name="input" v-model="mobile.bgPhone" disabled="don" style="text-align:right "></input>
				</view>
				<view class="cu-form-group">
					<view class="title">座机号码:</view>
					<input name="input" v-model="mobile.zjPhone" disabled="don" style="text-align:right "></input>
				</view> -->
				<view class="cu-form-group">
					<view class="title" style="font-size: 15px;">办公地址:</view>
					<input name="input" v-model="mobile.officeAddress" disabled="don" style="text-align:right;font-size: 15px;  "></input>
				</view>
				<view class="cu-form-group">
					<view class="title" style="font-size: 15px;">家庭地址:</view>
					<input name="input" v-model="mobile.familyAddress" disabled="don" style="text-align:right;font-size: 15px;  "></input>
				</view>
				<!-- !!!!! placeholder 在ios表现有偏移 建议使用 第一种样式 -->
				<view class="cu-form-group align-start">
					<view class="title" style="font-size: 15px;">备注:</view>
					<textarea maxlength="-1" @input="textareaBInput" v-model="mobile.remarks" disabled="don" style="text-align:right;font-size: 15px;  "></textarea>
				</view>
			</form>
		</view>

		<view style="padding: 30rpx 0 0 30rpx;color: #333333;border-top: 0.5px solid #eee;">联系记录
			<button class="cu-btn cuIcon bg-white round lg sm " @click="tianjia">
				<text class="cuIcon-add shadow"></text>
			</button>
			<!-- （<span>2</span>） -->
		</view>
		<view class="cu-items radius shadow-warp bg-white margin-top" v-for="(item, index) in kw2" v-if="kw2.length > 0" :key="index">
			<view class="cu-list menu-avatar">
				<view class="cu-item">
					<view class="content">
						<view class="text-blue">
							<view class="text-cut">联系人-{{ item.contactName }}</view>
						</view>
						<view class="text">
							<view class="text-cut">客户-{{ item.clientName }}</view>
						</view>
						<view class="text">
							<view class="text-cut">商机-{{ item.businessName }}</view>
						</view>
						<view class="text-gray text-sm flex">
							<view class="text-cut">联系内容:{{ item.callContent }}</view>
						</view>
						<view class="text-gray text-sm flex">
							<view class="text-cut">联系时间 : </view>
							<view class="text-cut">{{ item.callTime }}</view>
						</view>
					</view>
				</view>
			</view>
		</view>
	</view>
</template>

<script>
	import uniPopup from '@/components/uni-popup/uni-popup.vue';
	import uniIcon from '@/components/uni-icon/uni-icon.vue';
	import {
		GetContactCallLogList,
		AppShowContactGuid,
	} from '@/api/clien/personal.js';
	export default {
		components: {
			uniIcon,
			uniPopup
		},
		data() {
			return {
				kw2: [],
				don: "on",
				modalName: null,
				textareaBValue: '',
				modifyMobile: false,
				id: 0,
				clientUuid: "",
				contactName: "",
				clientName: "",
				contactPersonUuid: "",
				title: "",
				call: "",
				phone: "",
				email: "",
				sex: "",
				weChat: "",
				tencentQQ: "",
				bgPhone: "",
				zjPhone: "",
				officeAddress: "",
				familyAddress: "",
				remarks: "",
				mobile: {
					clientUuid: "",
					contactName: "",
					clientName: "",
					title: "",
					call: "",
					phone: "",
					email: "",
					sex: "",
					weChat: "",
					dateBirth:"",
					tencentQq: "",
					bgPhone: "",
					zjPhone: "",
					officeAddress: "",
					familyAddress: "",
					remarks: "",
					id: "",
				},

				mobile: [],
			};
		},
		methods: {
			back() {
				this.$router.go(-1);
			},
			textareaBInput(e) {
				this.textareaBValue = e.detail.value
			},

			hideModal(e) {
				this.modalName = null;
				uni.redirectTo({
					url: '/pages/duoduoCRM/personal/personal',
				});
			},
			tianjia() {
				//console.log(this.mobile.contactName);
				//this.contactName = this.kw[0].contactName;
				uni.navigateTo({
					//url: '/pages/duoduoCRM/personal/personal',
					url: '/pages/duoduoCRM/calllog/addcalllog2?contactPersonUuid=' + this.contactPersonUuid + '&contactName=' + this
						.mobile.contactName + '&clientUuid=' + this.mobile.clientUuid +  '&clientName=' + this.mobile.clientName
				});
			}
		},
		onLoad(opt) {
			this.id = opt.id;
			this.contactPersonUuid = opt.contactPersonUuid;
			let data = {
				contactPersonUuid: opt.contactPersonUuid
			};
			AppShowContactGuid(data).then(res => {
				//console.log(res);
				this.mobile = res.data.data[0];
				console.log(this.mobile);
			});
			GetContactCallLogList(data).then(res => {
				this.kw2 = res.data.data;
				console.log(this.kw2);
			});
		}
	}
</script>

<style>
	.cu-form-group {
		height: 90rpx;
	}

	.cu-form-group .title {
		min-width: calc(4em + 15px);
	}

	.container999 {
		width: 100vw;
		font-size: 28upx;
		/* min-height: 100vh; */
		min-height: 825px;
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
