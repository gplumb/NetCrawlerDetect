pipeline {
    agent { label 'ubuntu' }
    parameters {
        gitParameter defaultValue: 'origin/main', name: 'TAG', type: 'PT_TAG', sortMode: 'DESCENDING_SMART'
    }
    stages {
        stage('Checkout') {
            steps {
                echo "Checkout ${params.TAG}"
                checkout scm
            }
        }
        stage('Build solution') {
            steps {
                sh 'dotnet build'
            }
        }
        stage('Test solution') {
            steps {
                sh 'dotnet test --logger "trx;LogFileName=unit_tests.xml"'
                xunit checksName: '', tools: [xUnitDotNet(excludesPattern: '', pattern: '*/TestResults/*.xml', stopProcessingIfError: true)]
            }
        }
        stage('Define tag name') {
            steps {
                script {
                    env.TagName = params.TAG
                }
                echo "$TagName"
            }
        }
        stage('Create pack') {
            steps {
                sh 'dotnet pack NetCrawlerDetect/NetCrawlerDetect/NetCrawlerDetect.csproj -c Release -o ./Package'
            }
        }
        stage('Push pack to github') {
            steps {
                sh 'dotnet nuget push "./Package/JSport.NetCrawlerDetect.${env.TagName}.nupkg" --source "github" --force-english-output -k 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'
            }
        }
        stage('Remove pack from folder') {
            steps {
                sh 'rm -rf ./Package'
            }
        }
    }
    
    post {
        success {
            slackSend channel: 'serverci', color: "good", message:"Build completed successfully - ${env.JOB_NAME} ${env.BUILD_NUMBER} (<${env.BUILD_URL}|Open>)"
        }
        failure {
            slackSend channel: 'serverci', color: "danger", failOnError:true, message:"Build failed - ${env.JOB_NAME} ${env.BUILD_NUMBER} (<${env.BUILD_URL}|Open>)"
        }
    }
}