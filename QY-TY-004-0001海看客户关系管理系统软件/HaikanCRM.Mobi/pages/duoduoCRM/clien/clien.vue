<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" v-on:click="back">
			<block slot="backText">返回</block>
			<block slot="content">客户管理</block>
		</cu-custom>

		<view class="container999">
			<view class="cu-bar search bg-white" style="margin: 15rpx 0;padding: 15rpx 0;">
				<view class="search-form round">
					<text class="cuIcon-search"></text>
					<input v-model="mohupp" @focus="InputFocus" @blur="InputBlur" :adjust-position="false" type="text"
					 placeholder="客户名称" confirm-type="search"></input>
				</view>
				<view class="action">
					<button class="cu-btn bg-blue shadow-blur round" @click="cctv(mohupp)">搜索</button>
				</view>
			</view>
			<!-- 	<view class="cu-item" v-for="(item, index) in kw" v-if="kw.length > 0" :key="index" style="width: 100%;">
				<view class="content" style="width: 100%;">
					<view class="card" @click="clickPogodaID( item.clientUuid )">
						<view class="tt">
						</view>
						<view class="co-blue" style="font-weight: bold;">{{ item.clientName }}</view>
						<view>
							<text style="color: #0081FF;">客户经理：</text>{{ item.clientManager }}
						</view>
						<view>
							<text style="color: #0081FF;">地址：</text>{{ item.clientAddress }}
						</view>
						<view>
							<view class="cu-tag bg-yellow sm round" @click="clickPogodaID( item.clientUuid )">详情</view>
						</view>
					</view>
				</view>
			</view> -->
			<view class="content">
				<mix-tree :list="list" :params="treeParams" @treeItemClick="treeItemClick"></mix-tree>
			</view>
		</view>
		<button class="cu-btn cuIcon bg-blue round lg" style="position: fixed;bottom: 30px;right: 30px;" @click="geturl">
			<text class="cuIcon-add shadow"></text>
		</button>
	</view>
</template>

<script>
	import mixTree from '@/components/mix-tree/mix-tree';
	import uniPopup from '@/components/uni-popup/uni-popup.vue';
	import uniIcon from '@/components/uni-icon/uni-icon.vue';
	import {
		GetPogodaList,
	} from '@/api/clien/clien.js';
	export default {
		components: {
			uniIcon,
			uniPopup,
			mixTree
		},
		data() {
			return {
				mohupp:'',
				InputBottom: 0,
				StatusBar: this.StatusBar,
				CustomBar: this.CustomBar,
				hidden: true,
				listCurID: '',
				stopView: true,
				list: [],
				listCur: '',
				stores: {
					pogoda: {
						Kk1: '',
						id: 0,
						clientName: '',
						clientUuid: '',
						systemUserUuid: ''
					}
				},
				kw: [],
				treeParams: {
					defaultIcon: '/static/i2.png', // 默认图标
					currentIcon: '/static/i1.png', // 展开图标
					lastIcon: '/static/i3.png', //最后一级图标
					border: true // 边框， 默认不显示
				}
			}
		},
		methods: {
			back() {
				this.$router.go(-1);
			},
			geturl() {
				uni.navigateTo({
					url: '/pages/duoduoCRM/clien/addclien'
				})
			},
			InputFocus(e) {
				console.log(e);
				this.InputBottom = e.detail.height;
				this.mohupp = e.detail.value;
			},
			InputBlur(e) {
				console.log(e);
				this.InputBottom = 0;
				this.mohupp = e.detail.value;
			},

			clickPogodaID(clientUuid) {
				uni.navigateTo({
					url: '/pages/duoduoCRM/clien/infoclien?clientUuid=' + clientUuid
				});
				//console.log(id);
			},
			cctv(e) {
				console.log(e);
				uni.redirectTo({
					url: '/pages/duoduoCRM/clien/clien?kk1=' + e
				});
				// this.list = [];
				// console.log(9879);
				// console.log(this.list);
				// this.stores.pogoda.systemUserUuid = getApp().globalData.SystemUserUuid;
				// GetPogodaList(this.stores.pogoda).then(res => {
				// 	if (res.data.data.listData == '') {
				// 		uni.showToast({
				// 			title: '未匹配到相关客户信息',
				// 			icon: 'none'
				// 		});
				// 		return;
				// 	} else {
				// 		this.kw = res.data.data.listData;
				// 		this.list = res.data.data.listData;

				// 	}
				// });
			},
			//点击最后一级时触发该事件
			treeItemClick(item) {
				let {
					id,
					name,
					parentId
				} = item;
				uni.showModal({
					content: `点击了${parentId.length+1}级菜单, ${name}, id为${id}, 父id为${parentId.toString()}`
				})
				//console.log(item)
			}
		},
		onLoad(opt) {
			this.stores.pogoda.id = opt.id;
			this.stores.pogoda.clientName = opt.clientName;
			this.stores.pogoda.clientUuid = opt.clientUuid;
			this.stores.pogoda.SystemUserUuid = getApp().globalData.SystemUserUuid;
			this.stores.pogoda.Kk1 = opt.kk1;
			//console.log(JSON.stringify(this.stores.pogoda));
			GetPogodaList(JSON.stringify(this.stores.pogoda)).then(res => {
				console.log(res.data.data.listData);
				if (res.data.data.listData == '') {
					uni.showToast({
						title: '暂未查到客户信息',
						icon: 'none'
					});
					return;
				} else {
					this.kw = res.data.data.listData;
					this.list = res.data.data.listData;
					//console.log(this.kw.length);
				}
			});
		}
	};
</script>

<style>
	.card {
		margin: 30rpx;
	}

	.indexes {
		position: relative;
	}

	.indexBar {
		position: fixed;
		right: 0px;
		bottom: 0px;
		padding: 20upx 20upx 20upx 60upx;
		display: flex;
		align-items: center;
	}

	.indexBar .indexBar-box {
		width: 40upx;
		height: auto;
		background: #fff;
		display: flex;
		flex-direction: column;
		box-shadow: 0 0 20upx rgba(0, 0, 0, 0.1);
		border-radius: 10upx;
	}

	.indexBar-item {
		flex: 1;
		width: 40upx;
		height: 40upx;
		display: flex;
		align-items: center;
		justify-content: center;
		font-size: 24upx;
		color: #888;
	}

	movable-view.indexBar-item {
		width: 40upx;
		height: 40upx;
		z-index: 9;
		position: relative;
	}

	movable-view.indexBar-item::before {
		content: "";
		display: block;
		position: absolute;
		left: 0;
		top: 10upx;
		height: 20upx;
		width: 4upx;
		background-color: #f37b1d;
	}

	.indexToast {
		position: fixed;
		top: 0;
		right: 80upx;
		bottom: 0;
		background: rgba(0, 0, 0, 0.5);
		width: 100upx;
		height: 100upx;
		border-radius: 10upx;
		margin: auto;
		color: #fff;
		line-height: 100upx;
		text-align: center;
		font-size: 48upx;
	}

	.container999 {
		width: 100vw;
		font-size: 28upx;
		overflow: hidden;
		color: #6b8082;
		position: relative;
		/* top:50px; */
		background-color: #f6f6f6;
	}

	.content {
		width: 100%;
	}

	.card {
		background-color: white;
		margin: 30rpx;
		background: #ffffff;
		box-shadow: 0 0 5px 0 rgba(0, 0, 0, 0.1);
		border-radius: 5px;
		position: relative;
		padding: 30rpx;
		line-height: 60rpx;
	}

	.tt {
		font-size: 30upx;
		font-weight: bold;
	}

	.co-blue {
		color: #007AFF;
	}
</style>
