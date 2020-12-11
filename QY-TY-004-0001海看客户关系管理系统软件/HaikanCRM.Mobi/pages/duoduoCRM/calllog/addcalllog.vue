<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" v-on:click="back">
			<block slot="backText">返回</block>
			<block slot="content">添加</block>
		</cu-custom>
		<form>
			<view class="cu-form-group" style="height: 90rpx;" v-show="clienid==0">
				<view class="title" style="font-size: 15px;">*客户名称:</view>
				<input name="input" v-model="mobile.clientName" disabled="on" style="text-align:right; font-size: 15px;"></input>

			</view>
<!-- 			<view class="cu-form-group" style="height: 90rpx;" >
				<view class="title">客户名称:</view>
				<picker @change="PickerChange3" :value="Prioritylist1.index2" :range="Prioritylist1.picker" range-key="clientName" disabled="false">
					<view class="picker">
						{{ Prioritylist1.index2 > -1 ? Prioritylist1.picker[Prioritylist1.index2].clientName : '请选择客户' }}
					</view>
				</picker>
			</view> -->
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">*联系人</view>
				<picker @change="PickerChange2" :value="Prioritylist.contactPersonUuid" :range="Prioritylist.picker" range-key="contactName">
					<view class="picker">
						{{Prioritylist.index2>-1?Prioritylist.picker[Prioritylist.index2].contactName:'请选择联系人'}}
					</view>
				</picker>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">选择商机</view>
				<picker @change="PickerChange5" :value="Prioritylist5.businessUuid" :range="Prioritylist5.picker5" range-key="businessName">
					<view class="picker">
						{{Prioritylist5.index5>-1?Prioritylist5.picker5[Prioritylist5.index5].businessName:'请选择商机'}}
					</view>
				</picker>
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
				<view class="title" style="font-size: 15px;">*联系内容:</view>
				<textarea name="input" v-model="mobile.callContent" style="height: 700rpx; font-size: 15px; " maxlength="-1"></textarea>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">联系方式</view>
				<picker @change="PickerChange4" :value="index4" :range="picker4">
					<view class="picker">
						{{index4>-1?picker4[index4]:'请选择联系方式'}}
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
		AppEditCallLog,
		gealluser,
		GetSelectCustomer,
		GetCusIdSelectBus,
	} from '@/api/clien/calllog.js';
	export default {
		components: {
			uniIcon,
			uniPopup
		},
		data() {
			return {
				Prioritylist5: {
					picker5: [{
						businessUuid: "",
						businessName: "",
					}],
					index5: -1
				},
				picker4: [
					'微信', 'QQ', '电话','面谈','邮件'
				],
				index4: -1,
				//客户
				Prioritylist1: {
					picker: [{
						clientUuid: '',
						clientName: ''
					}],
					index2: -1
				},
				clienid: 0,
				clientUuid: "",
				clientName: "",
				//客户
				Prioritylist: {
					picker: [{
						contactPersonUuid: "",
						contactName: "",
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
					callTime: formateDate(new Date(), 'Y-M-D h:min'),
					callLogUuid: "",
					clientName: "",
					clientUuid: "",
					contactPersonUuid: "",
					id: "",
					contactDetailsUuid: "",
					contactDetailsText: "",
					contactName: "",
					businessUuid: "",
					businessName: "",
				},
			};
		},
		methods: {
			back() {
				this.$router.go(-1);
			},
			textareaBInput(e) {
				this.textareaBValue = e.detail.value
			},
			TimeChange(e) {
				this.mobile.callTime = e.detail.value
			},
			PickerChange2(e) {
				console.log(e);
				this.Prioritylist.index2 = e.detail.value
				this.mobile.contactPersonUuid = this.Prioritylist.picker[this.Prioritylist.index2].contactPersonUuid
			},
			PickerChange4(e) {
				this.index4 = e.detail.value
				this.mobile.contactDetailsUuid = this.index4
				console.log(this.mobile.contactDetailsUuid);
			},
			PickerChange5(e) {
				console.log(e);
				this.Prioritylist5.index5 = e.detail.value;
				this.mobile.businessUuid = this.Prioritylist5.picker5[this.Prioritylist5.index5].businessUuid;
			},
			PickerChange3(e) {
				this.Prioritylist.picker = [];
				this.Prioritylist.index2 = -1;
				this.Prioritylist1.index2 = e.target.value;
				this.clientUuid = this.Prioritylist1.picker[this.Prioritylist1.index2].clientUuid;
				let data = {
					clientUuid: this.clientUuid
				};
				gealluser(data).then(res => {
					this.Prioritylist.picker = res.data.data
				});
			},
			geturl() {
				//console.log(1);
				if (this.mobile.contactPersonUuid == '') {
					uni.showToast({
						title: '请选择联系人',
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

				let data = {
					ContactPersonUuid: this.mobile.contactPersonUuid,
					ContactName: this.mobile.contactName,
					ClientUuid: this.clientUuid,
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
				AppEditCallLog(JSON.stringify(data)).then(res => {
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
							title: '修改失败,请检查所输入信息是否合法',
							icon: 'none'
						});
						return;
					}
				});
			},
			quxiao() {
				uni.redirectTo({
					url: '/pages/duoduoCRM/clien/clien',
				});
			}
		},
		onLoad(opt) {
			if (opt.clientUuid == null || opt.clientUuid == '' || opt.clientUuid == undefined) {
				this.clienid = 1;
				GetSelectCustomer().then(res => {
					this.Prioritylist1.picker = res.data.data;
					console.log(this.Prioritylist1.picker)
				});
			} else {
				this.clienid = 0;
				this.clientUuid = opt.clientUuid;
				this.clientName = opt.clientName;
				this.mobile.clientName = this.clientName;
				console.log(this.clientName);
				let data = {
					clientUuid: opt.clientUuid,
					guid: getApp().globalData.SystemUserUuid
				};
				gealluser(data).then(res => {
					this.Prioritylist.picker = res.data.data
				});
				GetCusIdSelectBus(data).then(res => {
					this.Prioritylist5.picker5 = res.data.data
				});
			}


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
