const { reactive,nextTick  } = Vue
const {ElMessage} = ElementPlus

const apptwo = Vue.createApp({
    setup() {
        // 响应式数据
        const comments = ref([]);
        const total = ref(0);
        const page = ref(1);
        const pageSize = ref(5);
        const search = ref('');
        const sortBy = ref('');
        const refreshLoading = ref(true);
        const replyComment = ref(null);
        const otpInterval = ref(null);
        const ruleFormRef = ref(null);
        const contentRef = ref(null);
        const ReplyEmail = ref(null)
        const form = reactive({
            parentId: '',
            postId: POST_ID,
            userName: '',
            email: '',
            url: null,
            content: '',
        });
        const formRules = reactive({
            userName: [
                { required: true, message: '请输入用户名称', trigger: 'blur' },
                { min: 2, max: 20, message: '长度在 2 到 20 个字符', trigger: 'blur' }
            ],
            email: [
                {required: true, message: '请输入邮箱', trigger: 'blur'},
                {type: 'email', message: '邮箱格式不正确'}
            ],
            url: [
                {type: 'url', message: `请输入正确的url`, trigger: 'blur'},
            ],
            content: [
                {required: true, message: '请输入评论内容', trigger: 'blur'},
                {min: 1, max: 300, message: '长度 在 1 到 300 个字符', trigger: 'blur'},
            ]
        });

        // 方法

        async function getAnonymousUser(email, otp) {
            try {
                const response = await axios.get(`/Api/Comment/GetAnonymousUser?email=${email}&otp=${otp}`);
                return response.data;
            } catch (error) {
                return error.response.data;
            }
        }

        async function getComments() {
            const params = {
                PostId: POST_ID,
                Page: page.value,
                PageSize: pageSize.value,
            };
            if (search.value) params.Search = search.value;
            if (sortBy.value) params.SortBy = sortBy.value;

            try {
                const response = await axios.get(`/Api/Comment`, { params });
                comments.value = response.data.data.map(e => {
                    // 处理数据
                    let item = {
                        ...e,
                        anonymousUser: {
                            ...e.anonymousUser,
                            createdTime: dayjs(e.anonymousUser.createdTime).format('YYYY-MM-DD'),
                            updatedTime: dayjs(e.anonymousUser.updatedTime).format('YYYY-MM-DD'),
                        },
                        createdTime: dayjs(e.createdTime).format('YYYY-MM-DD'),
                        updatedTime: dayjs(e.updatedTime).format('YYYY-MM-DD'),
                        avatar:`http://q2.qlogo.cn/headimg_dl?dst_uin=${e.anonymousUser.email.split('@')[0]}&spec=100`
                    }

                    if (e.parent) {
                        item.replyUser = e.parent.anonymousUser.name
                    }
                    return item
                });
                total.value = response.data.pagination.totalItemCount;
            } catch (error) {
                // 处理错误
                ElMessage.error(error.message)
            }
        }

        async function submitComment (data) {
            try {
                const response = await axios.post(`/Api/Comment`, { ...data });
                return response.data;
            } catch (error) {
                return error.response.data;
            }
        }

        async function handleRefresh() {
            refreshLoading.value = true
            await getComments();
            await nextTick();
            refreshLoading.value = false
        }
        
        async function handleReset(formEl){
            if (!formEl) return
            formEl.resetFields()
        }
        
        async function handleSubmit(formEl){
            if (!formEl) return
            await formEl.validate(async (valid, fields) => {
                if (valid) {
                    if(form.parentId !== ""){
                        let data = reactive({
                            name:form.userName,
                            email:ReplyEmail.value,
                            content:form.content,
                            postId:form.postId
                        })
                        await SendReplyEmail(data)
                    }
                    let res =await submitComment(form)
                    if(res.successful){
                        ElMessage.success(res.message)
                        let email = form.email
                        await handleReset(formEl)
                        form.email = email
                        
                        ReplyEmail.value = ""
                        await handleReplyTagClose()
                    }else{
                        ElMessage.error(res.message)
                    }
                    await getComments()
                } else {
                    ElMessage("请填写必要参数！")
                }
            })
        }
        
        async function handleSizeChange(value){
            pageSize.value = value
            await handleRefresh()
        }
        async function handleCurrentChange(value){
            page.value = value
            await handleRefresh()
        }

        async function handleReply(comment){
            replyComment.value = comment
            form.parentId = comment.id
            
            ReplyEmail.value = comment.anonymousUser.email
            
            console.log(ReplyEmail.value)
            contentRef.value.focus();
        }
        
        async function handleReplyTagClose(){
            form.parentId = ''
            replyComment.value = null
        }
        
        async function SendReplyEmail(data){
            try {
                const response = await axios.post(`/Api/Comment/Send`, { ...data });
                return response.data;
            } catch (error) {
                return error.response.data;
            }
        }
        // 初始化数据
        getComments();
        refreshLoading.value = false;

        // 返回数据和方法
        return {
            comments,
            total,
            page,
            pageSize,
            search,
            sortBy,
            refreshLoading,
            replyComment,
            otpInterval,
            form,
            formRules,
            handleRefresh,
            handleReset,
            handleSubmit,
            handleSizeChange,
            handleCurrentChange,
            contentRef,
            ruleFormRef,
            handleReply,
            ReplyEmail,
            SendReplyEmail
            // 其他方法
        };
    }
});

apptwo.use(ElementPlus);
apptwo.mount('#vue-comment');
