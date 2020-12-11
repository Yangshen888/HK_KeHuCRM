<template>
	<view>
		<messages v-if="PageCur=='messages'"></messages>
		<gognneng v-if="PageCur=='gognneng'"></gognneng>
		<communList v-if="PageCur=='communList'"></communList>
		<information v-if="PageCur=='information'"></information>
		<view class="cu-bar tabbar bg-white shadow foot">
			<view class="action" @click="NavChange" data-cur="messages">
				<view class='cuIcon-cu-image'>
					<image :src="'/static/tabbar/plugin' + [PageCur=='messages'?'_cur':''] + '.png'"></image>
				</view>
				<view :class="PageCur=='messages'?'text-green':'text-gray'">消息</view>
			</view>
			<view class="action" @click="NavChange" data-cur="gognneng">
				<view class='cuIcon-cu-image'>
					<image :src="'/static/tabbar/basics' + [PageCur == 'gognneng'?'_cur':''] + '.png'"></image>
				</view>
				<view :class="PageCur=='gognneng'?'text-green':'text-gray'">功能</view>
			</view>
			<view class="action text-gray add-action">
				<button class="cu-btn cuIcon-add bg-green shadow" @tap="showModal" data-target="gridModal"></button>
				新建
			</view>
			<view class="action" @click="NavChange" data-cur="communList">
				<view class='cuIcon-cu-image'>
					<image :src="'/static/tabbar/component' + [PageCur == 'communList'?'_cur':''] + '.png'"></image>
				</view>
				<view :class="PageCur=='communList'?'text-green':'text-gray'">通讯录</view>
			</view>
			<view class="action" @click="NavChange" data-cur="information">
				<view class='cuIcon-cu-image'>
					<image :src="'/static/tabbar/about' + [PageCur == 'information'?'_cur':''] + '.png'"></image>
				</view>
				<view :class="PageCur=='information'?'text-green':'text-gray'">个人信息</view>
			</view>
		</view>
		<view class="cu-modal" :class="modalName=='gridModal'?'show':''" @tap="hideModal" style="background: rgba(0, 0, 0, 0.7);">
			<view class="cu-dialog" @tap.stop style="background-color: transparent;">
				<view class="cu-list menu text-left solid-top">
					<view class="cu-item" style="background-color: transparent;height: 340px;">
						<view class="content">
							<button class="cu-btn round btnclick" @click="getcal" style="background-color: transparent;height: 100%;">
								<image src="../../../static/duoduo/iconkehu.png" style="height: 50px;width: 50px;"></image>
							</button>
							<button class="cu-btn round btnclick" @click="getper" style="background-color: transparent;height: 100%;">
								<image src="../../../static/duoduo/iconlianxi.png" style="height: 50px;width: 50px;"></image>
							</button>
							<button class="cu-btn round btnclick" @click="getBus" style="background-color: transparent;height: 100%;">
								<image src="../../../static/duoduo/iconshangji.png" style="height: 50px;width: 50px;"></image>
							</button>
							<view style="margin-left: 30px;color: #f0f0f0;">新建客户</view>
							<view style="margin-left: 125px;margin-top: -23px;color: #f0f0f0;">新建联系人</view>
							<view style="margin-left: 230px;margin-top: -23px;color: #f0f0f0;">新建商机</view>
							<button class="cu-btn round btnclick" style="margin-top: 15px;background-color: transparent;height: 100%;"
							 @click="getjilu">
								<image src="../../../static/duoduo/iconjilu.png" style="height: 50px;width: 50px;"></image>
							</button>
							<view style="margin-left: 15px;color: #f0f0f0;">新建联系记录</view>
						</view>
					</view>
				</view>
			</view>
		</view>
	</view>
</template>

<script>
	import {
		getuserinfo,
		GetSystemUser
	} from '@/api/clien/calllog.js';
	export default {
		data() {
			return {
				PageCur: 'messages',
				modalName: null,
			}
		},

		onLoad() {
			console.log("开始");
			dd.getAuthCode({
				success: (res) => {
					var code = res.authCode; //授权码
					console.log("很高兴获得了授权请求码：", code);
					console.log("appkey：", this.appkey);
					getuserinfo(code).then(res => {
						console.log(111111);
						console.log(res);
						getApp().globalData.UserId = res.data.data.user.streets;
						getApp().globalData.UserName = res.data.data.user.realName;
						uni.setStorageSync('token',res.data.data.token);
						console.log('输出token');
						console.log(uni.getStorageSync('token'));
						//console.log(res.data.data[0].userid);
						//console.log(getApp().globalData.UserId);
						let data=getApp().globalData.UserId;
						console.log(data);
						GetSystemUser(data).then(res=>{
							console.log(res);
							getApp().globalData.SystemUserUuid = res.data.data.systemUserUuid;
							getApp().globalData.realName = res.data.data.realName;
						});
					});
				}
			});
		},
		methods: {
			getcal() {
				uni.navigateTo({
					url: '/pages/duoduoCRM/clien/addclien'
				});
			},
			getper() {
				uni.navigateTo({
					url: '/pages/duoduoCRM/personal/addpersonal'
				});
			},
			getBus() {
				uni.navigateTo({
					url: '/pages/duoduoCRM/business/addbusiness'
				});
			},
			getjilu() {
				uni.navigateTo({
					url: '/pages/duoduoCRM/calllog/newaddcall'
				});
			},
			showModal(e) {
				this.modalName = e.currentTarget.dataset.target
			},
			hideModal(e) {
				this.modalName = null
			},
			NavChange: function(e) {
				console.log(this.PageCur);
				this.PageCur = e.currentTarget.dataset.cur
			}
		}
	}
</script>

<style>
	.classmodel {
		height: 93%;
		background-color: #f1f1f1;
	}

	.btnclick {
		background-color: transparent;
		margin-left: 20px;
		height: 100%;
	}

	.cu-modal::before {
		filter: blur(5px);
	}
</style>
