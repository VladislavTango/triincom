<template>
  <a-layout style="min-height: 100vh">
    <a-layout-header class="header">
      <div class="logo">Triincom</div>
      <a-menu
        mode="horizontal"
        theme="dark"
        :selected-keys="[$route.path]"
        style="flex: 1"
      >
        <a-menu-item key="/" @click="$router.push('/')">Список заявок</a-menu-item>
        <a-menu-item key="/add" @click="$router.push('/add')">Добавить заявку</a-menu-item>
      </a-menu>
    </a-layout-header>

    <a-layout-content style="padding: 24px">
      <router-view />
    </a-layout-content>
    
  </a-layout>
</template>

<script setup>
</script>

<style scoped>
.logo {
  color: #fff;
  font-size: 20px;
  font-weight: bold;
  margin-right: 24px;
}
.header {
  display: flex;
  align-items: center;
  background: #001529;
}
</style>
