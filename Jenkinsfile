pipeline {
    agent { label 'ubuntu' }
    parameters {
        gitParameter defaultValue: 'origin/main', name: 'TAG', type: 'PT_TAG', sortMode: 'DESCENDING_SMART'
    }
	environment {
        GITHUB_TOKEN = credentials('GitHub-JSport')
		TAGNAME = "${params.TAG}"
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
                sh 'dotnet build NetCrawlerDetect/NetCrawlerDetect.sln'
            }
        }
        stage('Test solution') {
            steps {
                sh 'dotnet test NetCrawlerDetect/NetCrawlerDetect.sln --logger "trx;LogFileName=unit_tests.xml"'
                xunit checksName: '', tools: [MSTest(excludesPattern: '', pattern: '**/TestResults/*.xml', stopProcessingIfError: true)]
            }
        }
        stage('Create pack') {
            steps {
                sh 'dotnet pack NetCrawlerDetect/NetCrawlerDetect/NetCrawlerDetect.csproj -c Release -o ./Package'
            }
        }
        stage('Push pack to github') {
            steps {
                sh 'dotnet nuget push "./Package/JSport.NetCrawlerDetect.${TAGNAME}.nupkg" --source "https://nuget.pkg.github.com/j-sport/index.json" --force-english-output -k ${GITHUB_TOKEN}'
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