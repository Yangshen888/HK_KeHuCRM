<template>
	<view style="padding-top: 0;">
		<cu-custom bgColor="bg-gradual-blue"><block slot="content">通讯录</block></cu-custom>
		<view class="cu-bar search bg-white" style="top:5px;">
			<view class="search-form round">
				<text class="cuIcon-search"></text>
				<input  type="text" placeholder="商机名称" confirm-type="search" />
			</view>
			<view class="action"><button class="cu-btn bg-blue shadow-blur round" >搜索</button></view>
		</view>
		<scroll-view scroll-y class="indexes" :scroll-into-view="'indexes-'+ listCurID" :style="[{height:'calc(100vh - '+ CustomBar + 'px - 50px)'}]"
		 :scroll-with-animation="true" :enable-back-to-top="true">
			<block v-for="(item,index) in list" :key="index">
				<view :class="'indexItem-' + item.name" :id="'indexes-' + item.name" :data-index="item.name">
					<view class="padding">{{item.name}}</view>
					<view class="cu-list menu-avatar no-padding">
						<view class="cu-item" v-for="(items,sub) in 2" :key="sub">
							<view class="cu-avatar round lg">{{item.name}}</view>
							<view class="content">
								<view class="text-grey">{{item.name}}<text class="text-abc">{{list[sub].name}}</text>君</view>
								<view class="text-gray text-sm">
									有{{sub+2}}个主子需要伺候
								</view>
							</view>
						</view>
					</view>
				</view>
			</block>
		</scroll-view>
		<view class="indexBar" :style="[{height:'calc(100vh - ' + CustomBar + 'px - 50px)'}]">
			<view class="indexBar-box" @touchstart="tStart" @touchend="tEnd" @touchmove.stop="tMove">
				<view class="indexBar-item" v-for="(item,index) in list" :key="index" :id="index" @touchstart="getCur" @touchend="setCur"> {{item.name}}</view>
			</view>
		</view>
		<!--选择显示-->
		<view v-show="!hidden" class="indexToast">
			{{listCur}}
		</view>
	</view>
</template>

<script>
</script>

<style>
</style>
