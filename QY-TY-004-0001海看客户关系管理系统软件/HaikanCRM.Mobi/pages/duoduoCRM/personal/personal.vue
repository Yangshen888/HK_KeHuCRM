<template>
	<view>
		<meta name="format-detection" content="telephone=yes" />
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" v-on:click="back">
			<block slot="backText">返回</block>
			<block slot="content">联系人列表</block>
		</cu-custom>

		<view class="container999">
			<view class="cu-bar search bg-white" style="margin: 15rpx 0;padding: 15rpx 0;">
				<view class="search-form round">
					<text class="cuIcon-search"></text>
					<input v-model="stores.pogoda.kk1" @focus="InputFocus" @blur="InputBlur" :adjust-position="false" type="text"
					 placeholder="联系人名称" confirm-type="search"></input>
				</view>
				<view class="action">
					<button class="cu-btn bg-blue shadow-blur round" @click="cctv()">搜索</button>
				</view>
			</view>
			<view class="cu-item" v-for="(item, index) in kw" v-if="kw.length > 0" :key="index" style="width: 100%;">
				<view class="content" style="width: 100%;">
					<view class="card">
						<view class="tt">
							<text style="color: #0081FF;">联系人：</text><text class="co-blue">{{ item.contactName }}</text>
						</view>
						<view>
							<text style="color: #0081FF;">所属客户：</text>{{ item.clientName }}
						</view>

						<view>
							
							<text style="color: #0081FF;">联系电话：</text>
							<view v-for="(item1,index1) in item.phone">
								<!-- <a :href="'tel:' + item1">{{item1}}</a> -->
								<view @click ="getmakePhoneCall(item1)">{{item1}}</view>
							</view>
						</view>

						<view>
							<view class="cu-tag bg-yellow round sm " @click="clickPogodaID(item.contactPersonUuid )">详情</view>
							<view class="cu-tag bg-green sm round" @click="clickEditID( item.id )">编辑</view>
						</view>
					</view>
				</view>
			</view>
		</view>
		<button class="cu-btn cuIcon bg-blue round lg" style="position: fixed;bottom: 30px;right: 30px;" @click="geturl">
			<text class="cuIcon-add shadow"></text>
		</button>
	</view>
</template>

<script>
	import uniPopup from '@/components/uni-popup/uni-popup.vue';
	import uniIcon from '@/components/uni-icon/uni-icon.vue';
	import {
		GetList,
	} from '@/api/clien/personal.js';
	export default {
		components: {
			uniIcon,
			uniPopup
		},
		data() {
			return {
				InputBottom: 0,
				// StatusBar: this.StatusBar,
				// CustomBar: this.CustomBar,
				// hidden: true,
				// listCurID: '',
				// list: [],
				// listCur: '',
				stores: {
					pogoda: {
						kk1: "",
						id: 0,
						contactName: '',
						contactPersonUuid: '',
						clientName: '',
						SystemUserUuid: '',

					}
				},
				kw: [],
			}
		},
		methods: {
			back() {
				this.$router.go(-1);
			},
			geturl() {
				uni.navigateTo({
					url: '/pages/duoduoCRM/personal/addpersonal'
				})
			},
			InputFocus(e) {
				this.InputBottom = e.detail.height
			},
			InputBlur(e) {
				this.InputBottom = 0
			},

			clickPogodaID(contactPersonUuid) {
				//console.log(contactPersonUuid);
				uni.redirectTo({
					url: '/pages/duoduoCRM/personal/infopersonal?contactPersonUuid=' + contactPersonUuid
				});
			},
			clickEditID(id) {
				uni.redirectTo({
					url: '/pages/duoduoCRM/personal/editpersonal?id=' + id
				});
			},
			cctv() {
				GetList(JSON.stringify(this.stores.pogoda)).then(res => {
					console.log(res);
					if (res.data.data.listData == '') {
						uni.showToast({
							title: '未匹配到联系人',
							icon: 'none'
						});
						return;
					} else {
						this.kw = res.data.data.listData;
						//console.log(this.kw.length);
					}
				});
			},
			getmakePhoneCall(phone) {
				console.log(phone);
				 //window.location.href = 'tel://' + phone;
				
				// // 导入Activity、Intent类
				// var Intent = plus.android.importClass("android.content.Intent");
				// var Uri = plus.android.importClass("android.net.Uri");
				// // 获取主Activity对象的实例  
				// var main = plus.android.runtimeMainActivity();
				// // 创建Intent  
				// var uri = Uri.parse("tel:" + phone); // 这里可修改电话号码  
				// var call = new Intent("android.intent.action.CALL", uri);
				// // 调用startActivity方法拨打电话  
				// main.startActivity(call);
				
				dd.showCallMenu({
				    phoneNumber: phone.toString(), // 期望拨打的电话号码
				    code: '+86', // 国家代号，中国是+86
				    showDingCall: true, // 是否显示钉钉电话
				    success:function(res){   
						console.log('调用成功!')
				    },
				    fail:function(err){
						console.log('调用失败!' + err )
				    }
				});
				
				// uni.makePhoneCall({
				// 	// 手机号
				// 	phoneNumber: phone.toString(),
				// 	// 成功回调
				// 	success: (res) => {
				// 		console.log('调用成功!')
				// 	},
				// 	// 失败回调
				// 	fail: (res) => {
						
				// 		console.log('调用失败!' + res )
				// 		console.log(res)
				// 	}
				// });
			},
		},
		onLoad(opt) {
			let that = this;
			console.log("进入联系人");
			this.stores.pogoda.id = opt.id;
			this.stores.pogoda.contactPersonUuid = opt.contactPersonUuid;
			this.stores.pogoda.contactName = opt.contactName;
			this.stores.pogoda.SystemUserUuid = getApp().globalData.SystemUserUuid;
			//JSON.stringify(this.stores.pogoda)
			GetList(JSON.stringify(this.stores.pogoda)).then(res => {
				console.log(res);
				if (res.data.data.listData == '') {
					uni.showToast({
						title: '暂未查到联系人信息',
						icon: 'none'
					});
					return;
				} else {
					that.kw = res.data.data.listData;
				}
			});

		}
	};
</script>

<style>
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
		min-height: 100vh;
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
