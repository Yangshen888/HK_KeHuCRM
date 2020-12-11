<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" v-on:click="back">
			<block slot="backText">返回</block>
			<block slot="content">添加</block>
		</cu-custom>
		<form>

			<view class="cu-form-group" style="height: 90rpx;">
				<view class="title" style="font-size: 15px;">*客户名称:</view>
				<str-autocomplete :stringList="company" :importvalue="title" @select="selectOne" @change="textChange"
				 highlightColor="#FF0000" style="font-size: 15px;" v-model="title"></str-autocomplete>
			</view>
			<view class="cu-form-group">
				<view class="title" style="font-size: 15px;">*联系人</view>
				<picker  @change="PickerChange2" :value="Prioritylistlxr.contactPersonUuid" :range="Prioritylistlxr.pickerlxr"
				 range-key="contactName">
					<view class="picker">
						{{Prioritylistlxr.index2>-1?Prioritylistlxr.pickerlxr[Prioritylistlxr.index2].contactName:'请选择联系人'}}
					</view>
				</picker>
			</view>
			<!-- @click="fansilxr"@click="fansisj" -->
			<view class="cu-form-group" >
				<view class="title" style="font-size: 15px;">选择商机</view>
				<picker @change="PickerChange5" :value="Prioritylist5.businessUuid" :range="Prioritylist5.picker5"
				 range-key="businessName">
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
				<textarea name="input" v-model="mobile.callContent" style="height: 700rpx;font-size: 15px; " maxlength="-1"></textarea>
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
				<input name="input" v-model="mobile.contactDetailsText" style="text-align:right;font-size: 15px;"></input>
			</view> -->

		</form>

		<view class="padding flex flex-direction">
			<button class="cu-btn bg-blue margin-tb-sm lg" style=" border-radius:20px;" @tap="geturl">确定</button>

			<!-- <button class="cu-btn bg-gray margin-tb-sm lg" @tap="quxiao">取消</button> -->
		</view>
	</view>
</template>

<script>
	import strAutocomplete from '@/components/str-autocomplete/str-autocomplete.vue';
	import http from '@/components/utils/http.js'
	import {
		formateDate
	} from '@/global/utils.js';

	import uniPopup from '@/components/uni-popup/uni-popup.vue';
	import uniIcon from '@/components/uni-icon/uni-icon.vue';
	import {
		AppAddCallLog,
		gealluser,
		GetSelectCustomer,
		PassCusSelectCon,
		PassCusSelectBus,
	} from '@/api/clien/calllog.js';
	import {
		VagueSelectBusiness,
		VagueSelectContact,
		VagueSelectCustomer,
		GetPogodaList,
	} from '@/api/clien/clien.js';
	export default {
		components: {
			strAutocomplete,
			uniIcon,
			uniPopup
		},
		data() {
			return {
				title: '',
				company: [],
				company1: [],
				company2: [],
				//商机
				Prioritylist5: {
					picker5: [{
						businessUuid: "",
						businessName: "",
					}],
					index5: -1
				},
				//联系人
				Prioritylistlxr: {
					pickerlxr: [{
						contactPersonUuid: "",
						contactName: "",
					}],
					index2: -1
				},
				picker4: [
					'微信', 'QQ', '电话', '面谈', '邮件'
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
			selectOne(opt) {
				this.clientName = opt;
				this.fansisj(opt);
				this.fansilxr(opt)
			},

			fansilxr(e) {
				console.log(654656);
				this.Prioritylistlxr.pickerlxr = [];
				let data = {
					clientNames: e
				};
				PassCusSelectCon(JSON.stringify(data)).then(res => {
					//console.log(res);
					this.Prioritylistlxr.pickerlxr = res.data.data;
					console.log(this.Prioritylistlxr.pickerlxr);
					// console.log(this.Prioritylist.picker);
				});
			},
			fansisj(e) {
				let data = {
					clientNames: e
				};
				PassCusSelectBus(JSON.stringify(data)).then(res => {
					//console.log(res);
					this.Prioritylist5.picker5 = [];
					this.Prioritylist5.picker5 = res.data.data;
				});
			},

			textChange(opt) {
				this.clientName = opt;
				//console.log(this.clientName);
				//console.log("textchange:" + opt)
			},
			TimeChange(e) {
				this.mobile.callTime = e.detail.value
			},
			back() {
				this.$router.go(-1);
			},
			textareaBInput(e) {
				this.textareaBValue = e.detail.value
			},
			PickerChange2(e) {
				this.Prioritylistlxr.index2 = e.detail.value
				this.mobile.contactPersonUuid = this.Prioritylistlxr.pickerlxr[this.Prioritylistlxr.index2].contactPersonUuid
			},

			PickerChange5(e) {
				console.log(e);
				this.Prioritylist5.index5 = e.detail.value
				this.mobile.businessUuid = this.Prioritylist5.picker5[this.Prioritylist5.index5].businessUuid
			},
			PickerChange4(e) {
				this.index4 = e.detail.value
				this.mobile.contactDetailsUuid = this.index4
				console.log(this.mobile.contactDetailsUuid);
			},

			geturl() {
				//console.log(1);
				if (this.clientName == '') {
					uni.showToast({
						title: '请选择客户',
						icon: 'none',
						mask: true,
						duration: 1000
					});
					return;
				};
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
					ClientName: this.clientName,
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
				console.log(data);
				AppAddCallLog(JSON.stringify(data)).then(res => {
					console.log(res);
					this.info = res.data.message;
					// this.modalName = 'show';
					if (this.info == '成功') {
						uni.showToast({
							title: '添加成功',
							icon: 'none'
						});
						uni.navigateTo({
							url: '/pages/duoduoCRM/calllog/calllog'
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
			VagueSelectCustomer().then(res => {
				this.company = res.data.data;
				for (let i = 0; i < res.data.data.length; i++) {
					this.company[i] = res.data.data[i].clientName
				}
				//console.log(this.company);
			});



		},

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
