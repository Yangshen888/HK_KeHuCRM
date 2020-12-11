<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" v-on:click="back">
			<block slot="backText">返回</block>
			<block slot="content">联系记录</block>
		</cu-custom>
		<view class="container999">
			<view class="cu-bar search bg-white" style="margin: 15rpx 0;padding: 15rpx 0;">
				<view class="search-form round">
					<text class="cuIcon-search"></text>
					<input v-model="stores.pogoda.contactName" @focus="InputFocus" @blur="InputBlur" :adjust-position="false" type="text"
					 placeholder="输入联系人名称" confirm-type="search"></input>
				</view>
				<view class="action">
					<button class="cu-btn bg-blue shadow-blur round" @click="cctv()">搜索</button>
				</view>
			</view>
<!-- 			<view class="cu-card article" v-for="(item, index) in kw" v-if="kw.length > 0" :key="index">
				<view class="cu-list menu-avatar">
					<view class="cu-item">
						<view class="content" @click="infoclick(item.callLogUuid)">
							<view class="text-gray text-sm flex">
								<view class="text-cut  text-sm">联系时间 : {{ item.callTime }}</view>
							</view>
							<view class="text-blue">
								<view class="text-cut">客户-{{ item.clientName }}</view>>
							</view>
							<view class="text">
								<view class="text-cut">联系人-{{ item.contactName }}</view>
							</view>
							<view class="text">
								<view class="text-cut"> 商机-{{ item.businessName }}</view>
							</view>
														<view class="text-gray text-sm flex">
								<view class="text-cut">联系内容:</view>
								<view class="text-cut">{{ item.callContent }}</view>
							</view>

						</view>

					</view>
				</view>
			</view> -->
			
			<view class="cu-item" v-for="(item, index) in kw" v-if="kw.length > 0" :key="index" style="width: 100%;">
				<view class="content" style="width: 100%;">
					<view class="card"  @click="infoclick(item.callLogUuid)">
						<view class="text-gray text-sm flex">
							<view class="text-cut  text-sm">联系时间 : {{ item.callTime }}</view>
						</view>
						<view>
							<text style="color: #0081FF;">客户-</text><text class="co-blue">{{ item.clientName }}</text>
						</view>
						<view>
							<text style="color: #0081FF;">联系人-</text>{{ item.contactName }}
						</view>
						<view>
							<text style="color: #0081FF;">商机-</text>{{ item.businessName }}
						</view>

					</view>
				</view>
			
			</view>
			<button class="cu-btn cuIcon bg-blue round lg" style="position: fixed;bottom: 30px;right: 30px;" @click="geturl">
				<text class="cuIcon-add shadow"></text>
			</button>
		</view>
	</view>
</template>

<script>
	import uniPopup from '@/components/uni-popup/uni-popup.vue';
	import uniIcon from '@/components/uni-icon/uni-icon.vue';
	import {
		GetPogodaList,
	} from '@/api/clien/calllog.js';
	export default {
		components: {
			uniIcon,
			uniPopup
		},
		data() {
			return {
				InputBottom: 0,
				stores: {
					pogoda: {
						logUuid: "",
						id: 0,
						contactName: "",
						SystemUserUuid: ''
					}
				},
				kw: [],
			}
		},
		methods: {
			back() {
				this.$router.go(-1);
			},
			infoclick(e) {
				uni.navigateTo({
					url: '/pages/duoduoCRM/calllog/infoCallLog?callLogUuid=' + e
				});
			},
			geturl() {
				uni.navigateTo({
					url: '/pages/duoduoCRM/calllog/newaddcall'
				})
			},
			getjilu() {
				uni.navigateTo({
					url: '/pages/duoduoCRM/calllog/newaddcall'
				});
			},
			InputFocus(e) {
				this.InputBottom = e.detail.height
			},
			InputBlur(e) {
				this.InputBottom = 0
			},
			cctv() {
				this.stores.pogoda.SystemUserUuid = getApp().globalData.SystemUserUuid;
				GetPogodaList(JSON.stringify(this.stores.pogoda)).then(res => {
					console.log(res.data.data);
					if (res.data.data == '') {
						uni.showToast({
							title: '未匹配到此联系人相关-联系记录',
							icon: 'none'
						});
						return;
					} else {
						this.kw = res.data.data;
						console.log(this.kw.length);
					}
				});
			}
		},
		onLoad(opt) {
			if(opt.logUuid!=undefined){
			this.stores.pogoda.logUuid = opt.logUuid;
			}
			//this.stores.pogoda.logUuid = "C5664A9D-B82C-4E06-A186-EC84A149A918";
			// JSON.stringify(this.stores.pogoda)
			GetPogodaList(JSON.stringify(this.stores.pogoda)).then(res => {
				console.log(res.data.data);
				if (res.data.data == '') {
					uni.showToast({
						title: '未查到联系记录',
						icon: 'none'
					});
					return;
				} else {
					this.kw = res.data.data;
					console.log(this.kw.length);
				}
			});
		}
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
