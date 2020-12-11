<template>
	
	<view class="content">
		<!-- 轮播 -->
	<swiper class="screen-swiper" :class="dotStyle?'square-dot':'round-dot'" :indicator-dots="true" :circular="true"
	 :autoplay="true" interval="5000" duration="500">
		<swiper-item>
			<image class="screen-swiper active" src="../../../static/image/huandengpian.png"></image>
		</swiper-item>
	
	</swiper>
		<!-- 功能 -->
		<scroll-view scroll-y class="page">
			<view class="nav-list">
				<navigator hover-class='none' :url="'/pages/' + item.name" class="nav-li" navigateTo :class="'bg-'+item.color"
				 :style="[{animation: 'show ' + ((index+1)*0.2+1) + 's 1'}]" v-for="(item,index) in elements" :key="index">
					<view class="nav-title">{{item.title}}</view>
				</navigator>
			</view>
			<view class="cu-tabbar-height"></view>
		</scroll-view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				elements: [{
						title: '联系人',
						name: 'duoduoCRM/personal/personal',
						color: 'blue',
						// cuIcon: 'vipcard',											
					},
					{
							title: '联系记录',
							name: 'duoduoCRM/calllog/calllog',
							color: 'blue',
							// cuIcon: 'vipcard',											
						},
					{
						title: '客户',
						name: 'duoduoCRM/clien/clien',
						color: 'blue',
						// cuIcon: 'formfill'					
					},
					{
						title: '商机',
						name: 'duoduoCRM/business/business',
						color: 'blue',
						// cuIcon: 'tagfill'
					},
					// {
					// 	title: '个人中心',
					// 	name: 'duoduoCRM/information/information',
					// 	color: 'blue',
					// 	// cuIcon: 'tagfill'
					// }
				],
				cardCur: 0,
				swiperList: [{
					id: 0,
					type: 'image',
					url: '/static/images/lbt1.jpg'
				}, {
					id: 1,
					type: 'image',
					url: '/static/images/lbt2.jpg',
				}, {
					id: 2,
					type: 'image',
					url: '/static/images/lbt3.jpg'
				}, {
					id: 3,
					type: 'image',
					url: '/static/images/lbt4.jpg'
				}, {
					id: 4,
					type: 'image',
					url: '/static/images/lbt5.jpg'
				}, {
					id: 5,
					type: 'image',
					url: '/static/images/lbt6.jpg'
				}
				// , {
				// 	id: 6,
				// 	type: 'image',
				// 	url: '/static/images/lbt7.jpg'
				// },
				],
				dotStyle: false,
				towerStart: 0,
				direction: '',
				appkey: 'dingzmb57bl0xjo3hsmu',
				appsecret: 't6nHlawdDuVhGiU8pDUj_q0VHKFDzOqeq2qvgqdgyLB2TFDCM1nrAEckJYVxkOqh',
			}
		},
		mounted() {
			this.TowerSwiper('swiperList');
			uni.setNavigationBarTitle({
				title: "多多控智慧CRM"
			})


		},
		methods: {
			DotStyle(e) {
				this.dotStyle = e.detail.value
			},
			// cardSwiper
			cardSwiper(e) {
				this.cardCur = e.detail.current
			},
			// towerSwiper
			// 初始化towerSwiper
			TowerSwiper(name) {
				let list = this[name];
				for (let i = 0; i < list.length; i++) {
					list[i].zIndex = parseInt(list.length / 2) + 1 - Math.abs(i - parseInt(list.length / 2))
					list[i].mLeft = i - parseInt(list.length / 2)
				}
				this.swiperList = list
			},

			// towerSwiper触摸开始
			TowerStart(e) {
				this.towerStart = e.touches[0].pageX
			},

			// towerSwiper计算方向
			TowerMove(e) {
				this.direction = e.touches[0].pageX - this.towerStart > 0 ? 'right' : 'left'
			},

			// towerSwiper计算滚动
			TowerEnd(e) {
				let direction = this.direction;
				let list = this.swiperList;
				if (direction == 'right') {
					let mLeft = list[0].mLeft;
					let zIndex = list[0].zIndex;
					for (let i = 1; i < this.swiperList.length; i++) {
						this.swiperList[i - 1].mLeft = this.swiperList[i].mLeft
						this.swiperList[i - 1].zIndex = this.swiperList[i].zIndex
					}
					this.swiperList[list.length - 1].mLeft = mLeft;
					this.swiperList[list.length - 1].zIndex = zIndex;
				} else {
					let mLeft = list[list.length - 1].mLeft;
					let zIndex = list[list.length - 1].zIndex;
					for (let i = this.swiperList.length - 1; i > 0; i--) {
						this.swiperList[i].mLeft = this.swiperList[i - 1].mLeft
						this.swiperList[i].zIndex = this.swiperList[i - 1].zIndex
					}
					this.swiperList[0].mLeft = mLeft;
					this.swiperList[0].zIndex = zIndex;
				}
				this.direction = ""
				this.swiperList = this.swiperList
			}
		}
	}
</script>

<style>
	.page {
		margin-top: 10px;
	}

	.tower-swiper .tower-item {
		transform: scale(calc(0.5 + var(--index) / 10));
		margin-left: calc(var(--left) * 100upx - 150upx);
		z-index: var(--index);
	}

	/* 	.content {
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
	} */

	.logo {
		height: 200rpx;
		width: 200rpx;
		margin-top: 200rpx;
		margin-left: auto;
		margin-right: auto;
		margin-bottom: 50rpx;
	}

	.text-area {
		display: flex;
		justify-content: center;
	}

	.title {
		font-size: 36rpx;
		color: #8f8f94;
	}
	
	.nav-li{
		    padding: 50rpx 30rpx;
	}
	
	.bg-blue:nth-child(1){
		background-image: url(../../../static/image/anniu01.png);
		background-position:50%;
		background-size:cover;
		background-color: inherit;
	}
	.bg-blue:nth-child(2){
		background-image: url(../../../static/image/anniu022.jpg);
		background-position:50%;
		background-size:cover;
		background-color: inherit;
	}
	.bg-blue:nth-child(3){
		background-image: url(../../../static/image/anniu03.png);
		background-position:50%;
		background-size:cover;
		background-color: inherit;
	}
	.bg-blue:nth-child(4){
		background-image: url(../../../static/image/anniu04.png);
		background-position:50%;
		background-size:cover;
		background-color: inherit;
	}
	.bg-blue:nth-child(5){
		background-image: url(../../../static/image/anniu05.png);
		background-position:50%;
		background-size:cover;
		background-color: inherit;
	}
	
</style>
