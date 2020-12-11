<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" v-on:click="back">
			<block slot="backText">返回</block>
			<block slot="content">客户详细信息</block>
		</cu-custom>
		<view class="cu-card article" :class="isCard?'no-card':''">
			<view style="padding: 30rpx 0 0 30rpx;color: #333333;">基本信息</view>
			<view class="cu-item shadow-warp bg" v-for="(item, index) in kw" v-if="kw.length > 0" :key="index">
				<view class="title">
					<view class="text-cut" style="font-size: 35rpx;margin-top: 20rpx;font-weight: bold;">{{ item.clientName }}</view>
					<view class="text-cut"><text class="tt" >客户经理:</text>
						{{ item.clientManager }}</view>
<!-- 					<view class="text-cut"><text class="tt" >参与人:</text>
						{{ item.systemUserUuid }}</view> -->
					<view class="text-cut"><text class="tt" >客户状态：</text>
						{{ item.clientStatus }}</view>
<!-- 					<view class="text-cut"><text class="tt" style="color: #0081FF;">电话:</text>
						{{ item.clientPhone }}</view> -->
					<view class="text-cut"><text class="tt" >地址:</text>
						{{ item.clientAddress }}</view>
				</view>
				<view class="content" style="padding: 20rpx 30rpx;">
					<view class="desc">
						<view>
<!-- 							<view class="cu-tag bg-blue light sm round">联系人</view> -->
							<view class="cu-tag bg-blue sm round" @click="clickEditID( item.clientUuid )">>编辑</view>
							<view class="cu-tag bg-green sm round" @click="clickBusiness( item.clientUuid )">商机</view>
						</view>
					</view>
				</view>
			</view>
			<view style="padding: 30rpx 0 0 30rpx;color: #333333;">联系记录
				<button class="cu-btn cuIcon bg-white round lg sm " @click="tianjia">
					<text class="cuIcon-add shadow"></text>
				</button>
				<!-- （<span>2</span>） -->
			</view>
			<view class="cu-items radius shadow-warp bg-white margin-top" v-for="(item, index) in kw2" v-if="kw2.length > 0"
			 :key="index">
				<view class="cu-list menu-avatar">
					<view class="cu-item">
						<view class="content">
							<view class="text-blue">
								<view class="text-cut">联系人-{{ item.contactName }}</view>
							</view>
							<view class="text">
								<view class="text-cut">客户-{{ item.clientName }}</view>
							</view>
							<view class="text-gray text-sm flex">
								<view class="text-cut">联系内容:{{ item.callContent }}</view>
							</view>
							<view class="text-gray text-sm flex">
								<view class="text-cut">联系时间 : </view>
								<view class="text-cut">{{ item.callTime }}</view>
							</view>
						</view>
						<view class="action">
							<!-- <view class="youshang"><img src="@/static/image/bi.png" ></img></view> -->
							<!-- 							<view class="youxia"><img style="margin-right: 20rpx;" src="@/static/image/xiaoxi.png"><img src="@/static/image/zhuanfa.png"></view> -->
						</view>
					</view>
				</view>
			</view>
		</view>
	</view>
</template>

<script>
	import {
		GetPogodaID,
		GetCallLogList,
	} from '@/api/clien/clien.js';
	export default {
		data() {
			return {
				clientUuid:"",
				clientName:"",
				isCard: false,
				stores: {
					pogoda: {
						id: 0,
						addTime: '',
						clientUuid: '',
						businessUuid: '',
						callLogUuid: '',
						clientArea: '',
						clientEmail: '',
						clientFax: '',
						clientIndustry: '',
						clientName: '',
						clientIndustryNavigation: '',
						clientManager: '',
						clientManagerNavigation: '',
						clientPhone: '',
						clientPostcode: '',
						clientStatus: '',
						clientStatusNavigation: '',
						clientType: '',
						clientTypeNavigation: '',
						clientWebsite: '',
						companyUuid: '',
						clientAddress: '',
						systemUserUuid: '',
					}
				},
				kw: [],
				kw2: [],
			};
		},
		methods: {
			back() {
				this.$router.go(-1);
			},
			IsCard(e) {
				this.isCard = e.detail.value
			},
			clickEditID(clientUuid) {
				uni.navigateTo({
					url: '/pages/duoduoCRM/clien/editclien?clientUuid=' + clientUuid
				});
			},
			tianjia() {
				this.clientUuid = this.stores.pogoda.clientUuid;
				this.clientName = this.kw[0].clientName;
				//console.log(this.stores.pogoda.clientName);
				uni.navigateTo({
					url: '/pages/duoduoCRM/calllog/addcalllog?clientUuid=' + this.clientUuid + '&clientName=' + this.clientName
				});
			},
			clickBusiness(id){
				uni.navigateTo({
					url: '/pages/duoduoCRM/business/business?clientUuid=' + id
				});
			}

		},
		onLoad(opt) {
			this.stores.pogoda.id = opt.id;
			this.stores.pogoda.clientUuid = opt.clientUuid;
			this.stores.pogoda.clientName = opt.clientName;
			let data = {
				id: opt.id,
				clientUuid: opt.clientUuid
			};
			GetPogodaID(data).then(res => {
				//console.log(res);
				this.kw = res.data.data;

			});
			GetCallLogList(data).then(res => {
				this.kw2 = res.data.data;
				console.log(this.kw2);
			});
		}
	};
</script>

<style>
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
