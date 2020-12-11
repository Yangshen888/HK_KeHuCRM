<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" v-on:click="back">
			<block slot="backText">返回</block>
			<block slot="content">修改</block>
		</cu-custom>
		<form>
			<view class="cu-form-group" style="height: 90rpx;">
				<view class="title">客户名称:</view>
				<input name="input" v-model="mobile.clientName" disabled="on"></input>
			</view>
						<view class="cu-form-group">
				<view class="title">所属客户</view>
				<picker @change="PickerChange2" :value="Prioritylist.clientUuid" :range="Prioritylist.picker" range-key="clientName">
					<view class="picker">
						{{Prioritylist.index2>-1?Prioritylist.picker[Prioritylist.index2].clientName:'请选择客户'}}
					</view>
				</picker>
			</view>
			<view class="cu-form-group">
				<textarea name="input" v-model="mobile.callContent" style="height: 600rpx;"></textarea>
			</view>

		</form>

		<view class="padding flex flex-direction" >
			<button class="cu-btn bg-blue margin-tb-sm lg" @tap="geturl">确定</button>
			<button class="cu-btn bg-gray margin-tb-sm lg" @tap="quxiao">取消</button>
		</view>
	</view>
</template>

<script>
	import uniPopup from '@/components/uni-popup/uni-popup.vue';
	import uniIcon from '@/components/uni-icon/uni-icon.vue';
	import {
		GetCustomerCallLog,
		AppEditCallLog,
		gealluser,
	} from '@/api/clien/calllog.js';
	export default {
		components: {
			uniIcon,
			uniPopup
		},
		data() {
			return {
				clientUuid:"",
				clientName:"",
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
				mobile: {
					sheBeiId:"",
					callContent:"",
					callTime:"",
					callLogUuid:"",
					clientName:"",
					clientUuid:"",
					id: "",
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
			PickerChange2(e) {
				console.log(e);
				this.Prioritylist.index2 = e.detail.value
				this.mobile.clientUuid = this.Prioritylist.picker[this.Prioritylist.index2].clientUuid
			},

			geturl() {
				//console.log(1);
				let data = {
					ContactPersonUuid: this.mobile.contactPersonUuid,
					ContactName: this.mobile.contactName,
					ClientUuid: this.mobile.clientUuid,
					usGuid: getApp().globalData.SystemUserUuid,
					usName: getApp().globalData.UserName,
				};
				//console.log(data);
				AppEditCallLog(JSON.stringify(data)).then(res => {
					console.log(res);
					this.info = res.data.message;
					// this.modalName = 'show';
					if (this.info == '成功') {
						uni.showToast({
							title: '修改成功',
							icon: 'none'
						});
									uni.navigateTo({
										url: '/pages/duoduoCRM/personal/personal'
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
					url: '/pages/duoduoCRM/personal/personal',
				});
			}
		},
		onLoad(opt) {
			this.id = opt.id;
			this.clientUuid = opt.clientUuid;
			this.clientName = opt.clientName;
			this.id = opt.id;
			let data = {
				clientUuid: opt.clientUuid
			};
			gealluser(data).then(res => {
				this.Prioritylist.picker = res.data.data
			});
			//console.log(data.clientUuid)
			GetCustomerCallLog(data).then(res => {
				console.log(res);
				 this.mobile = res.data.data[0];
				// //console.log(this.Prioritylist.picker[0].clientUuid);
				// for (let i = 0; i < this.Prioritylist.picker.length; i++) {
				// 	if (this.Prioritylist.picker[i].clientUuid == this.mobile.clientUuid) {
				// 		this.Prioritylist.index2 = i;
				// 	}
				// }
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
