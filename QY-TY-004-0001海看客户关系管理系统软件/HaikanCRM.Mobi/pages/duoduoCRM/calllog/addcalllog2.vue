<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" v-on:click="back">
			<block slot="backText">返回</block>
			<block slot="content">添加</block>
		</cu-custom>
		<form>
			<view class="cu-form-group" style="height: 90rpx; font-size: 15px;">
				<view class="title" style="font-size: 15px;">*联系人:</view>
				<input name="input" v-model="mobile.contactName" disabled="on" style="text-align:right; font-size: 15px;"></input>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">*客户:</view>
				<input name="input" v-model="mobile.clientName" disabled="on" style="text-align:right; font-size: 15px;"></input>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">选择商机</view>
				<picker @change="PickerChange2" :value="Prioritylist.businessUuid" :range="Prioritylist.picker" range-key="businessName">
					<view class="picker">
						{{Prioritylist.index2>-1?Prioritylist.picker[Prioritylist.index2].businessName:'请选择商机'}}
					</view>
				</picker>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">*联系内容:</view>
				<textarea name="input" v-model="mobile.callContent" style="height: 700rpx; font-size: 15px;" maxlength="-1"></textarea>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">联系时间：</view>
				<picker mode="date" :value="mobile.callTime" @change="TimeChange">
					<view class="picker">
						{{mobile.callTime}}
					</view>
				</picker>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">联系方式</view>
				<picker @change="PickerChange1" :value="index1" :range="picker1">
					<view class="picker">
						{{index1>-1?picker1[index1]:'请选择联系方式'}}
					</view>
				</picker>
			</view>

<!-- 			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">联系号码:</view>
				<input name="input" v-model="mobile.contactDetailsText" style="text-align:right; font-size: 15px;"></input>
			</view> -->
		</form>

		<view class="padding flex flex-direction">
			<button class="cu-btn bg-blue margin-tb-sm lg" style="border-radius:20px;" @tap="geturl">确定</button>
			<!-- <button class="cu-btn bg-gray margin-tb-sm lg" @tap="quxiao">取消</button> -->
		</view>
	</view>
</template>

<script>
	import http from '@/components/utils/http.js'
	import {
		formateDate
	} from '@/global/utils.js';
	import uniPopup from '@/components/uni-popup/uni-popup.vue';
	import uniIcon from '@/components/uni-icon/uni-icon.vue';
	import {
		AppConSkipAddCallLog,
		GetCusIdSelectBus,
	} from '@/api/clien/calllog.js';
	export default {
		components: {
			uniIcon,
			uniPopup
		},
		data() {
			return {
				clientUuid: "",
				clientName: "",
				//客户
				Prioritylist: {
					picker: [{
						businessUuid: "",
						businessName: "",
					}],
					index2: -1
				},
				don: "on",
				modalName: null,
				textareaBValue: '',
				modifyMobile: false,
				index1: -1,
				id: 0,
				info: 'xxxxxxxx',
				mobile: {
					sheBeiId: "",
					callContent: "",
					callTime: "",
					callLogUuid: "",
					clientName: "",
					clientUuid: "",
					contactPersonUuid: "",
					id: "",
					contactName: "",
					businessUuid: "",
					businessName: "",
					callTime: formateDate(new Date(), 'Y-M-D h:min'),
					contactDetailsUuid: "",
					contactDetailsText: "",
				},
				picker1: [
					'微信', 'QQ', '电话','面谈','邮件'
				],
			};
		},
		methods: {
			back() {
				this.$router.go(-1);
			},
			textareaBInput(e) {
				this.textareaBValue = e.detail.value
			},
			PickerChange1(e) {
				this.index1 = e.detail.value
				this.mobile.contactDetailsUuid = this.index1
				console.log(this.mobile.contactDetailsUuid);
			},
			PickerChange2(e) {
				console.log(e);
				this.Prioritylist.index2 = e.detail.value
				this.mobile.businessUuid = this.Prioritylist.picker[this.Prioritylist.index2].businessUuid
			},
			//开始时间选择
			TimeChange(e) {
				this.mobile.callTime = e.detail.value
			},
			geturl() {

				if (this.mobile.clientUuid == '') {
					uni.showToast({
						title: '请选择客户',
						icon: 'none',
						mask: true,
						duration: 1000
					});
					return;
				};
				if (this.mobile.callContent == '') {
					uni.showToast({
						title: '请输入联系内容',
						icon: 'none',
						mask: true,
						duration: 1000
					});
					return;
				};
				//console.log(1);
				let data = {
					ContactPersonUuid: this.mobile.contactPersonUuid,
					//ContactName: this.mobile.contactName,
					ClientUuid: this.mobile.clientUuid,
					CallContent: this.mobile.callContent,
					callTime: this.mobile.callTime,
					contactDetailsUuid: this.mobile.contactDetailsUuid,
					//ClientUuid: this.mobile.clientUuid,
					contactDetailsText: this.mobile.contactDetailsText,
					//ClientUuid: this.mobile.clientUuid,
					businessUuid: this.mobile.businessUuid,
					usGuid: getApp().globalData.SystemUserUuid,
					usName: getApp().globalData.realName,
				};
				//console.log(data);
				AppConSkipAddCallLog(JSON.stringify(data)).then(res => {
					console.log(res);
					this.info = res.data.message;
					// this.modalName = 'show';
					if (this.info == '成功') {
						uni.showToast({
							title: '添加成功',
							icon: 'none'
						});
						uni.navigateBack({
							delta: 1,
							animationDuration: 200
						});
					} else {
						uni.showToast({
							title: '添加失败,请检查所输入信息是否合法',
							icon: 'none'
						});
						return;
					}
				});
			},
			quxiao() {
				uni.redirectTo({
					url: '/pages/duoduoCRM/personal/personal',
				});
			}
		},
		onLoad(opt) {
			this.contactPersonUuid = opt.contactPersonUuid;
			this.contactName = opt.contactName;
			this.mobile.contactName = opt.contactName;
			this.mobile.contactPersonUuid = opt.contactPersonUuid;
			this.mobile.clientUuid = opt.clientUuid;
			this.mobile.clientName = opt.clientName;
			console.log(this.contactPersonUuid);
			let data = {
				clientUuid: opt.clientUuid,
				guid: getApp().globalData.SystemUserUuid
			};
			GetCusIdSelectBus(data).then(res => {
				this.Prioritylist.picker = res.data.data
			});

		}
	}
</script>

<style>
	/* .cu-form-group {
		height: 90rpx;
	} */

	.cu-form-group .title {
		min-width: calc(4em + 15px);
	}
</style>
